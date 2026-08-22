using CurlinggoSoft.Hubs;
using CurlinggoSoft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CurlinggoSoft.Controllers
{
    // Chat bilateral Cliente <-> Técnico, atado a una reserva en curso.
    // Solo se permite mientras la reserva esté en un estado activo
    // (ASIGNADA, EN_CAMINO, EN_PROCESO) — antes de eso no hay técnico
    // asignado con quién chatear, y después (COMPLETADO/CANCELADA) ya no
    // aplica.
    [Authorize]
    public class MensajesController : Controller
    {
        private static readonly string[] EstadosActivos = { "ASIGNADA", "EN_CAMINO", "EN_PROCESO" };

        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificacionesHub> _hub;

        public MensajesController(ApplicationDbContext context, IHubContext<NotificacionesHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        private string UsuarioId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        // Valida solo pertenencia (cliente o técnico de la reserva), sin
        // importar el estado. Se usa para poder consultar el historial en
        // modo lectura incluso después de COMPLETADA/CANCELADA.
        private async Task<SolicitudReserva?> ObtenerReservaDelUsuarioAsync(long reservaId)
        {
            return await _context.SolicitudesReserva
                .Include(r => r.EstadoReserva)
                .FirstOrDefaultAsync(r => r.ReservaID == reservaId &&
                                          (r.ClienteID == UsuarioId || r.TecnicoID == UsuarioId));
        }

        // Valida pertenencia (cliente o técnico de la reserva) y estado activo.
        // Devuelve la reserva si todo es válido, o null si no aplica.
        // Se usa exclusivamente para permitir el ENVÍO de nuevos mensajes.
        private async Task<SolicitudReserva?> ObtenerReservaValidaAsync(long reservaId)
        {
            var reserva = await ObtenerReservaDelUsuarioAsync(reservaId);

            if (reserva is null) return null;
            if (reserva.EstadoReserva is null || !EstadosActivos.Contains(reserva.EstadoReserva.Codigo)) return null;
            return reserva;
        }

        // GET: /Mensajes/Historial?reservaId=123
        // Disponible siempre que el usuario pertenezca a la reserva, incluso
        // después de COMPLETADA/CANCELADA (modo solo lectura).
        [HttpGet]
        public async Task<IActionResult> Historial(long reservaId)
        {
            var reserva = await ObtenerReservaDelUsuarioAsync(reservaId);
            if (reserva is null) return StatusCode(403);

            var mensajes = await _context.MensajesReserva
                .Where(m => m.ReservaID == reservaId)
                .OrderBy(m => m.FechaEnvio)
                .ToListAsync();

            // Marcar como leídos los mensajes recibidos por el usuario actual
            var pendientes = mensajes.Where(m => m.ReceptorUsuarioID == UsuarioId && !m.Leido).ToList();
            if (pendientes.Count > 0)
            {
                foreach (var m in pendientes) m.Leido = true;
                await _context.SaveChangesAsync();
            }

            return PartialView("_ChatMensajes", mensajes.Select(m => new
            {
                m.MensajeID,
                m.Texto,
                m.FechaEnvio,
                esPropio = m.EmisorUsuarioID == UsuarioId
            }));
        }

        // GET: /Mensajes/EstadoActivo?reservaId=123
        // Le permite al cliente (JS) saber de antemano si todavia puede
        // enviar mensajes, para deshabilitar el input sin depender de un
        // intento fallido de POST /Mensajes/Enviar.
        [HttpGet]
        public async Task<IActionResult> EstadoActivo(long reservaId)
        {
            var reserva = await ObtenerReservaDelUsuarioAsync(reservaId);
            if (reserva is null) return StatusCode(403);

            var activo = reserva.EstadoReserva != null && EstadosActivos.Contains(reserva.EstadoReserva.Codigo);
            return Ok(new { activo });
        }

        // POST: /Mensajes/Enviar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enviar(long reservaId, string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return BadRequest("El mensaje no puede estar vacío.");

            var reserva = await ObtenerReservaValidaAsync(reservaId);
            if (reserva is null) return StatusCode(403);

            var receptorId = reserva.ClienteID == UsuarioId ? reserva.TecnicoID : reserva.ClienteID;
            if (string.IsNullOrEmpty(receptorId))
                return BadRequest("La reserva aún no tiene técnico asignado.");

            var mensaje = new MensajeReserva
            {
                ReservaID = reservaId,
                EmisorUsuarioID = UsuarioId,
                ReceptorUsuarioID = receptorId,
                Texto = texto.Trim(),
                FechaEnvio = DateTime.Now
            };

            _context.MensajesReserva.Add(mensaje);
            await _context.SaveChangesAsync();

            await _hub.Clients.Group(NotificacionesHub.GrupoReserva(reservaId))
                .SendAsync("NuevoMensaje", new
                {
                    reservaId,
                    mensaje.MensajeID,
                    mensaje.Texto,
                    mensaje.FechaEnvio,
                    emisorId = mensaje.EmisorUsuarioID
                });

            return Ok(new { mensaje.MensajeID, mensaje.FechaEnvio });
        }
    }
}
