using CurlinggoSoft.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CurlinggoSoft.Services
{
    public class DispatchEngineService : IDispatchEngineService
    {
        private readonly ApplicationDbContext _context;

        public DispatchEngineService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> GenerarOfertasLoteInicialAsync(long reservaId, int tamanoLote = 3)
        {
            // 1. Ejecutar el Match Predictivo (Filtro por especialidad, radio, horario y ranking)
            // Se utiliza el procedimiento almacenado existente en la base de datos.
            var reservaIdParam = new SqlParameter("@ReservaID", reservaId);
            var radioParam = new SqlParameter("@RadioKm", 20.00m); // Radio configurable
            var maxTecnicosParam = new SqlParameter("@MaxTecnicos", tamanoLote);

            // Creamos un modelo anónimo o DTO temporal para recibir los datos del SP
            var tecnicosCandidatos = await _context.Database
                .SqlQueryRaw<TecnicoCandidatoDto>(
                    "EXEC dbo.usp_Reserva_BuscarTecnicosDisponibles @ReservaID, @RadioKm, @MaxTecnicos",
                    reservaIdParam, radioParam, maxTecnicosParam)
                .ToListAsync();

            if (!tecnicosCandidatos.Any())
            {
                // No hay técnicos disponibles en este momento
                return false;
            }

            // 2. Generar el "Lote" de Ofertas (El Ping)
            // Por cada técnico que pasó el filtro, le creamos una oferta en la base de datos.
            foreach (var tecnico in tecnicosCandidatos)
            {
                var pReserva = new SqlParameter("@ReservaID", reservaId);
                var pTecnico = new SqlParameter("@TecnicoID", tecnico.TecnicoID);
                var pDistancia = new SqlParameter("@DistanciaMetros", tecnico.DistanciaMetros);
                var pExpiracion = new SqlParameter("@FechaExpiracion", DateTime.Now.AddSeconds(45)); // Tienen 45 seg para aceptar
                var pMensaje = new SqlParameter("@Mensaje", "¡Nuevo servicio disponible cerca de ti!");

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_OfertaTecnico_Crear @ReservaID, @TecnicoID, @DistanciaMetros, @FechaExpiracion, @Mensaje",
                    pReserva, pTecnico, pDistancia, pExpiracion, pMensaje);
            }

            return true;
        }
    }

    // DTO Interno para mapear el resultado del Procedimiento Almacenado
    public class TecnicoCandidatoDto
    {
        public string TecnicoID { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public decimal CalificacionPromedio { get; set; }
        public decimal DistanciaMetros { get; set; }
    }
}