using CurlinggoSoft.Hubs;
using CurlinggoSoft.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CurlinggoSoft.Services
{
    public class DispatchEngineService : IDispatchEngineService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificacionesHub> _hub;

        public DispatchEngineService(ApplicationDbContext context, IHubContext<NotificacionesHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        public async Task<bool> GenerarOfertasLoteInicialAsync(long reservaId, int tamanoLote = 3)
        {
            var reservaIdParam = new SqlParameter("@ReservaID", reservaId);
            var radioParam = new SqlParameter("@RadioKm", 20.00m);
            var maxTecnicosParam = new SqlParameter("@MaxTecnicos", tamanoLote);

            var tecnicosCandidatos = await _context.Database
                .SqlQueryRaw<TecnicoCandidatoDto>(
                    "EXEC dbo.usp_Reserva_BuscarTecnicosDisponibles @ReservaID, @RadioKm, @MaxTecnicos",
                    reservaIdParam, radioParam, maxTecnicosParam)
                .ToListAsync();

            if (!tecnicosCandidatos.Any())
            {
                return false;
            }

            var reserva = await _context.SolicitudesReserva
                .Include(r => r.Servicio)
                .FirstOrDefaultAsync(r => r.ReservaID == reservaId);

            var fechaExpiracion = DateTime.Now.AddSeconds(45);

            foreach (var tecnico in tecnicosCandidatos)
            {
                var pReserva = new SqlParameter("@ReservaID", reservaId);
                var pTecnico = new SqlParameter("@TecnicoID", tecnico.TecnicoID);
                var pDistancia = new SqlParameter("@DistanciaMetros", tecnico.DistanciaMetros);
                var pExpiracion = new SqlParameter("@FechaExpiracion", fechaExpiracion);
                var pMensaje = new SqlParameter("@Mensaje", "¡Nuevo servicio disponible cerca de ti!");

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_OfertaTecnico_Crear @ReservaID, @TecnicoID, @DistanciaMetros, @FechaExpiracion, @Mensaje",
                    pReserva, pTecnico, pDistancia, pExpiracion, pMensaje);

                // PUSH en tiempo real: el técnico se entera al instante, sin
                // esperar al próximo ciclo de polling. Payload liviano a
                // propósito — el cliente lo usa solo como disparador para
                // refrescar contra el servidor, que sigue siendo la fuente
                // de verdad.
                await _hub.Clients.Group(NotificacionesHub.GrupoTecnico(tecnico.TecnicoID))
                    .SendAsync("NuevaOferta", new
                    {
                        reservaId,
                        servicio = reserva?.Servicio?.NombreServicio ?? "Servicio técnico",
                        distanciaMetros = tecnico.DistanciaMetros,
                        expiraEn = fechaExpiracion.ToString("O")
                    });
            }

            return true;
        }
    }

    public class TecnicoCandidatoDto
    {
        public string TecnicoID { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public decimal CalificacionPromedio { get; set; }
        public decimal DistanciaMetros { get; set; }
    }
}
