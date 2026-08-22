using CurlinggoSoft.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CurlinggoSoft.Services
{
    // Encapsula la llamada a dbo.usp_Evaluacion_Crear, compartida entre
    // ClienteController (CalificarTecnico) y TecnicoController (CalificarCliente).
    // Se centraliza aquí para no duplicar el armado de parámetros que ya
    // existía en EvaluacionesController.Create.
    public class EvaluacionService
    {
        private readonly ApplicationDbContext _context;
        public EvaluacionService(ApplicationDbContext context) => _context = context;

        public async Task CrearAsync(long reservaId, string evaluadorUsuarioId, string? evaluadoUsuarioId,
            int? servicioId, int tipoEvaluacionId, byte puntuacion, string? comentario)
        {
            await using var connection = new SqlConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync();
            await using var command = new SqlCommand("dbo.usp_Evaluacion_Crear", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add(new SqlParameter("@ReservaID", SqlDbType.BigInt) { Value = reservaId });
            command.Parameters.Add(new SqlParameter("@EvaluadorUsuarioID", SqlDbType.NVarChar, 450) { Value = evaluadorUsuarioId });
            command.Parameters.Add(new SqlParameter("@EvaluadoUsuarioID", SqlDbType.NVarChar, 450) { Value = (object?)evaluadoUsuarioId ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@ServicioID", SqlDbType.Int) { Value = (object?)servicioId ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@TipoEvaluacionID", SqlDbType.Int) { Value = tipoEvaluacionId });
            command.Parameters.Add(new SqlParameter("@Puntuacion", SqlDbType.TinyInt) { Value = puntuacion });
            command.Parameters.Add(new SqlParameter("@Comentario", SqlDbType.NVarChar, 1000) { Value = (object?)comentario ?? DBNull.Value });

            await command.ExecuteScalarAsync();
        }
    }
}
