using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CurlinggoSoft.Hubs
{
    // Hub de notificaciones en tiempo real para técnicos.
    //
    // Cada técnico que se conecta se une automáticamente a un "grupo" con su
    // propio TecnicoID (el mismo Id de AspNetUsers/dbo.Usuarios). Así el
    // servidor puede empujarle mensajes SOLO a él, sin llevar un registro
    // manual de conexiones abiertas. Si el mismo técnico tiene el panel
    // abierto en dos pestañas o dos dispositivos, ambas conexiones caen en el
    // mismo grupo y ambas reciben el evento — es el comportamiento esperado.
    //
    // Ajusta "Tecnico" abajo si el nombre de tu rol en AspNetRoles es distinto.
    [Authorize(Roles = "Tecnico")]
    public class NotificacionesHub : Hub
    {
        private string TecnicoId =>
            Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        public override async Task OnConnectedAsync()
        {
            if (!string.IsNullOrEmpty(TecnicoId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, GrupoTecnico(TecnicoId));
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (!string.IsNullOrEmpty(TecnicoId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, GrupoTecnico(TecnicoId));
            }
            await base.OnDisconnectedAsync(exception);
        }

        // Nombre del grupo, centralizado aquí para que DispatchEngineService y
        // TecnicoController lo construyan siempre igual sin repetir el string.
        public static string GrupoTecnico(string tecnicoId) => $"tecnico-{tecnicoId}";
    }
}
