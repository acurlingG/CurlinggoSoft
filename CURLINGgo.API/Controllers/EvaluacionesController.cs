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
    public class EvaluacionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EvaluacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/evaluaciones
        [HttpPost]
        public async Task<IActionResult> CrearEvaluacion([FromBody] CrearEvaluacionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var reserva = await _context.SolicitudesReserva.FindAsync(dto.ReservaID);
            if (reserva == null)
                return NotFound(new { mensaje = "La reserva especificada no existe." });

            // 1. Verificar que la reserva esté completada (ID 5)
            if (reserva.EstadoReservaID != 5)
            {
                return BadRequest(new { mensaje = "Solo se pueden evaluar servicios que hayan sido marcados como completados." });
            }

            // 2. Validar que el usuario sea el cliente o el técnico de la reserva
            if (reserva.ClienteID != userId && reserva.TecnicoID != userId)
            {
                return StatusCode(403, new { mensaje = "No tiene permisos para evaluar esta reserva." });
            }

            // 3. NUEVA VALIDACIÓN: Prevenir evaluaciones duplicadas por la misma reserva y evaluador
            bool yaEvaluado = await _context.Evaluaciones
                .AnyAsync(e => e.ReservaID == dto.ReservaID && e.EvaluadorUsuarioID == userId);

            if (yaEvaluado)
            {
                return BadRequest(new { mensaje = "Ya has enviado una evaluación para esta reserva." });
            }

            // 4. Determinar a quién se está evaluando
            string evaluadoId = (userId == reserva.ClienteID) ? reserva.TecnicoID! : reserva.ClienteID;

            // 5. Ejecutar procedimiento almacenado con manejo de excepciones
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            EXEC dbo.usp_Evaluacion_Crear 
                @ReservaID = {dto.ReservaID},
                @EvaluadorUsuarioID = {userId},
                @EvaluadoUsuarioID = {evaluadoId},
                @TipoEvaluacionID = {dto.TipoEvaluacionID},
                @Puntuacion = {dto.Puntuacion},
                @Comentario = {dto.Comentario};
        ");

                return Ok(new { mensaje = "Evaluación registrada con éxito." });
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // GET: api/evaluaciones/tecnico/{tecnicoId}
        [HttpGet("tecnico/{tecnicoId}")]
        public async Task<IActionResult> GetEvaluacionesTecnico(string tecnicoId)
        {
            var evaluaciones = await _context.Evaluaciones
                .AsNoTracking()
                .Where(e => e.EvaluadoUsuarioID == tecnicoId && e.Activa)
                .OrderByDescending(e => e.FechaEvaluacion)
                .Select(e => new EvaluacionDetalleDto
                {
                    EvaluacionID = e.EvaluacionID,
                    ReservaID = e.ReservaID,
                    EvaluadorUsuarioID = e.EvaluadorUsuarioID,
                    EvaluadoUsuarioID = e.EvaluadoUsuarioID,
                    Puntuacion = e.Puntuacion,
                    Comentario = e.Comentario,
                    FechaEvaluacion = e.FechaEvaluacion
                })
                .ToListAsync();

            return Ok(evaluaciones);
        }

        // GET: api/evaluaciones/tecnico/{tecnicoId}/resumen
        [HttpGet("tecnico/{tecnicoId}/resumen")]
        public async Task<IActionResult> GetResumenTecnico(string tecnicoId)
        {
            var evaluaciones = await _context.Evaluaciones
                .Where(e => e.EvaluadoUsuarioID == tecnicoId && e.Activa)
                .ToListAsync();

            if (!evaluaciones.Any())
            {
                return Ok(new
                {
                    promedioGeneral = 0.0,
                    totalEvaluaciones = 0,
                    desglose = new { cinco = 0, cuatro = 0, tres = 0, dos = 0, uno = 0 }
                });
            }

            var total = evaluaciones.Count;
            var promedio = evaluaciones.Average(e => (double)e.Puntuacion);

            return Ok(new
            {
                promedioGeneral = Math.Round(promedio, 2),
                totalEvaluaciones = total,
                desglose = new
                {
                    cinco = evaluaciones.Count(e => e.Puntuacion == 5),
                    cuatro = evaluaciones.Count(e => e.Puntuacion == 4),
                    tres = evaluaciones.Count(e => e.Puntuacion == 3),
                    dos = evaluaciones.Count(e => e.Puntuacion == 2),
                    uno = evaluaciones.Count(e => e.Puntuacion == 1)
                }
            });
        }

        // GET: api/evaluaciones/reserva/{reservaId}/pendiente
        [HttpGet("reserva/{reservaId}/pendiente")]
        public async Task<IActionResult> EsEvaluacionPendiente(int reservaId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var reserva = await _context.SolicitudesReserva.FindAsync(reservaId);
            if (reserva == null)
                return NotFound(new { mensaje = "La reserva especificada no existe." });

            // Verificar que el usuario sea el cliente o el técnico de la reserva
            if (reserva.ClienteID != userId && reserva.TecnicoID != userId)
            {
                return StatusCode(403, new { mensaje = "No tiene permisos para consultar esta reserva." });
            }

            // Si la reserva no está completada (ID 5), no hay evaluación pendiente
            if (reserva.EstadoReservaID != 5)
            {
                return Ok(new
                {
                    pendiente = false,
                    razon = "La reserva aún no ha sido completada."
                });
            }

            // Verificar si el usuario ya registró su evaluación
            bool yaEvaluado = await _context.Evaluaciones
                .AnyAsync(e => e.ReservaID == reservaId && e.EvaluadorUsuarioID == userId);

            return Ok(new
            {
                pendiente = !yaEvaluado,
                reservaId = reservaId,
                evaluadoId = (userId == reserva.ClienteID) ? reserva.TecnicoID : reserva.ClienteID
            });
        }
    }
}