using CurlinggoSoft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CurlinggoSoft.Hubs
{
    // Hub de notificaciones en tiempo real, compartido entre técnicos y
    // clientes:
    //
    //   - Técnicos: se unen automáticamente al grupo "tecnico-{id}" en cuanto
    //     conectan (igual que antes). Reciben "NuevaOferta" y "OfertaYaTomada".
    //
    //   - Clientes: NO se unen automáticamente a nada (un cliente puede tener
    //     varias reservas históricas y no queremos suscribirlo a todas). En
    //     vez de eso, el JS de Paso6ConfirmacionExitosa llama explícitamente
    //     a SuscribirseAReserva(reservaId) apenas carga la página. Reciben
    //     "TecnicoAsignado" y "EstadoActualizado" solo de ESA reserva.
    //
    // Solo requiere estar autenticado (cualquier rol) — la restricción de
    // "solo puedes suscribirte a TU reserva" se valida dentro del método,
    // no a nivel de clase, porque ahora conviven ambos roles en el mismo hub.
    [Authorize]
    public class NotificacionesHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public NotificacionesHub(ApplicationDbContext context)
        {
            _context = context;
        }

        private string UsuarioId =>
            Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        private bool EsTecnico => Context.User?.IsInRole("Tecnico") ?? false;

        public override async Task OnConnectedAsync()
        {
            if (EsTecnico && !string.IsNullOrEmpty(UsuarioId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, GrupoTecnico(UsuarioId));
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (EsTecnico && !string.IsNullOrEmpty(UsuarioId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, GrupoTecnico(UsuarioId));
            }
            await base.OnDisconnectedAsync(exception);
        }

        // Llamado por el cliente desde Paso6ConfirmacionExitosa.cshtml, y
        // también por el técnico desde el chat de la reserva (_ChatReserva.cshtml).
        // Valida que la reserva le pertenezca (como cliente o como técnico
        // asignado) antes de unirlo al grupo — así nadie puede suscribirse a
        // la reserva de otra persona solo adivinando el ReservaID.
        public async Task SuscribirseAReserva(long reservaId)
        {
            var perteneceALaReserva = await _context.SolicitudesReserva
                .AnyAsync(r => r.ReservaID == reservaId &&
                               (r.ClienteID == UsuarioId || r.TecnicoID == UsuarioId));

            if (perteneceALaReserva)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, GrupoReserva(reservaId));
            }
        }

        // Chat bilateral cliente-técnico de una reserva activa. La
        // persistencia (INSERT en MensajesReserva) y la validación de que la
        // reserva esté en un estado activo (ASIGNADA/EN_CAMINO/EN_PROCESO)
        // ya se hicieron en MensajesController.Enviar ANTES de llamar a este
        // método — aquí solo se retransmite en tiempo real a los miembros
        // del grupo "reserva-{id}" (cliente + técnico ya están suscritos:
        // el técnico vía OnConnectedAsync/grupo propio + este grupo, el
        // cliente vía SuscribirseAReserva).
        public async Task NotificarNuevoMensaje(long reservaId, object mensaje)
        {
            await Clients.Group(GrupoReserva(reservaId)).SendAsync("NuevoMensaje", mensaje);
        }

        public static string GrupoTecnico(string tecnicoId) => $"tecnico-{tecnicoId}";
        public static string GrupoReserva(long reservaId) => $"reserva-{reservaId}";
    }
}
