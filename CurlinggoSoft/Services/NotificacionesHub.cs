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

        // Llamado por el cliente desde Paso6ConfirmacionExitosa.cshtml.
        // Valida que la reserva le pertenezca antes de unirlo al grupo — así
        // un cliente no puede suscribirse a la reserva de otra persona solo
        // adivinando el ReservaID.
        public async Task SuscribirseAReserva(long reservaId)
        {
            var esDelCliente = await _context.SolicitudesReserva
                .AnyAsync(r => r.ReservaID == reservaId && r.ClienteID == UsuarioId);

            if (esDelCliente)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, GrupoReserva(reservaId));
            }
        }

        public static string GrupoTecnico(string tecnicoId) => $"tecnico-{tecnicoId}";
        public static string GrupoReserva(long reservaId) => $"reserva-{reservaId}";
    }
}
