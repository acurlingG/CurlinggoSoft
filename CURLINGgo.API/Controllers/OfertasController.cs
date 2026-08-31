using System.Security.Claims;
using CURLINGgo.API.DTOs;
using CurlinggoSoft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CURLINGgo.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OfertasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OfertasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ofertas/pendientes
        [HttpGet("pendientes")]
        public async Task<IActionResult> GetOfertasPendientes()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var ofertas = await _context.OfertasTecnico
                .AsNoTracking()
                .Where(o => o.TecnicoID == userId && o.EstadoOfertaID == 1) // 1 = Pendiente
                .Select(o => new OfertaTecnicoResumenDto
                {
                    OfertaTecnicoID = o.OfertaTecnicoID,
                    ReservaID = o.ReservaID,
                    ServicioNombre = o.Reserva != null && o.Reserva.Servicio != null ? o.Reserva.Servicio.NombreServicio : "Servicio",
                    FechaHoraProgramada = o.Reserva != null ? o.Reserva.FechaHoraProgramada : DateTime.MinValue,
                    DireccionServicio = o.Reserva != null ? o.Reserva.DireccionServicio : string.Empty,
                    DistanciaMetros = o.DistanciaMetros,
                    MontoTotalCotizado = o.Reserva != null ? o.Reserva.MontoTotalCotizado : 0,
                    DescripcionProblema = o.Reserva != null ? o.Reserva.DescripcionProblema : string.Empty,
                    EstadoOfertaID = o.EstadoOfertaID,
                    FechaEnvio = o.FechaEnvio,
                    FechaExpiracion = o.FechaExpiracion
                })
                .ToListAsync();

            return Ok(ofertas);
        }

        // POST: api/ofertas/{id}/aceptar
        [HttpPost("{id}/aceptar")]
        public async Task<IActionResult> AceptarOferta(long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var oferta = await _context.OfertasTecnico
                .Include(o => o.Reserva)
                .FirstOrDefaultAsync(o => o.OfertaTecnicoID == id && o.TecnicoID == userId);

            if (oferta == null)
                return NotFound(new { mensaje = "Oferta no encontrada o no pertenece al técnico actual." });

            if (oferta.EstadoOfertaID != 1) // 1 = Pendiente
                return BadRequest(new { mensaje = "La oferta ya no está disponible para ser aceptada." });

            var reserva = oferta.Reserva;
            if (reserva == null)
                return NotFound(new { mensaje = "La reserva asociada a la oferta no existe." });

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int estadoAnteriorId = reserva.EstadoReservaID;
                int nuevoEstadoReservaId = 2; // 2 = Asignado / Aceptado por Técnico

                // 1. Actualizar oferta elegida
                oferta.EstadoOfertaID = 2; // 2 = Aceptada
                oferta.FechaRespuesta = DateTime.Now;

                // 2. Cancelar/Rechazar otras ofertas pendientes para la misma reserva
                var otrasOfertas = await _context.OfertasTecnico
                    .Where(o => o.ReservaID == reserva.ReservaID && o.OfertaTecnicoID != id && o.EstadoOfertaID == 1)
                    .ToListAsync();

                foreach (var otra in otrasOfertas)
                {
                    otra.EstadoOfertaID = 3; // 3 = Cancelada / Asignada a otro
                    otra.FechaRespuesta = DateTime.Now;
                }

                // 3. Asignar técnico a la solicitud de reserva
                reserva.TecnicoID = userId;
                reserva.EstadoReservaID = nuevoEstadoReservaId;

                // 4. Registrar la trazabilidad en HistorialEstadosReserva
                var historial = new HistorialEstadoReserva
                {
                    ReservaID = reserva.ReservaID,
                    EstadoAnteriorID = estadoAnteriorId,
                    EstadoNuevoID = nuevoEstadoReservaId,
                    FechaCambio = DateTime.Now,
                    UsuarioModificadorID = userId,
                    Observaciones = $"Oferta #{oferta.OfertaTecnicoID} aceptada por el técnico."
                };

                _context.HistorialEstadosReserva.Add(historial);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { mensaje = "Oferta aceptada con éxito y reserva asignada.", reservaId = reserva.ReservaID });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { mensaje = "Error al procesar la aceptación de la oferta.", detalle = ex.Message });
            }
        }

        // POST: api/ofertas/{id}/rechazar
        [HttpPost("{id}/rechazar")]
        public async Task<IActionResult> RechazarOferta(long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var oferta = await _context.OfertasTecnico
                .FirstOrDefaultAsync(o => o.OfertaTecnicoID == id && o.TecnicoID == userId);

            if (oferta == null)
                return NotFound(new { mensaje = "Oferta no encontrada." });

            if (oferta.EstadoOfertaID != 1)
                return BadRequest(new { mensaje = "La oferta ya ha sido procesada anteriormente." });

            oferta.EstadoOfertaID = 3; // 3 = Rechazada
            oferta.FechaRespuesta = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Oferta rechazada correctamente." });
        }
    }
}