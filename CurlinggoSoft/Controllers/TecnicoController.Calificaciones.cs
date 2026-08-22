using CurlinggoSoft.Models;
using CurlinggoSoft.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CurlinggoSoft.Controllers
{
    // Segunda parte de TecnicoController: calificación del cliente por parte
    // del técnico, una vez la reserva está COMPLETADO. Se separó en un
    // archivo de clase parcial para no tener que editar a ciegas el archivo
    // principal (414 líneas) y arriesgar romper el resto de las acciones.
    public partial class TecnicoController
    {
        // GET: /Tecnico/CalificarCliente?reservaId=123
        [HttpGet]
        public async Task<IActionResult> CalificarCliente(long reservaId)
        {
            var reserva = await _context.SolicitudesReserva
                .Include(r => r.EstadoReserva)
                .FirstOrDefaultAsync(r => r.ReservaID == reservaId && r.TecnicoID == TecnicoId);

            if (reserva is null || reserva.EstadoReserva?.Codigo != "COMPLETADA")
                return NotFound();

            var yaCalifico = await _context.Evaluaciones.AnyAsync(e =>
                e.ReservaID == reservaId &&
                e.EvaluadorUsuarioID == TecnicoId &&
                e.TipoEvaluacion!.Codigo == "TECNICO_A_CLIENTE");

            ViewData["ReservaID"] = reservaId;
            ViewData["YaCalifico"] = yaCalifico;
            ViewData["TituloCalificacion"] = "¿Cómo fue tu experiencia con el cliente?";
            ViewData["AccionEnviar"] = nameof(CalificarCliente);
            return PartialView("~/Views/Shared/_CalificacionPartial.cshtml");
        }

        // POST: /Tecnico/CalificarCliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalificarCliente(long reservaId, byte puntuacion, string? comentario,
            [FromServices] EvaluacionService evaluacionService)
        {
            var reserva = await _context.SolicitudesReserva
                .Include(r => r.EstadoReserva)
                .FirstOrDefaultAsync(r => r.ReservaID == reservaId && r.TecnicoID == TecnicoId);

            if (reserva is null || reserva.EstadoReserva?.Codigo != "COMPLETADA")
                return NotFound();

            try
            {
                var tipoId = await _context.TiposEvaluacion
                    .Where(t => t.Codigo == "TECNICO_A_CLIENTE")
                    .Select(t => t.TipoEvaluacionID)
                    .FirstOrDefaultAsync();

                await evaluacionService.CrearAsync(reservaId, TecnicoId, reserva.ClienteID,
                    reserva.ServicioID, tipoId, puntuacion, comentario);

                TempData["Success"] = "¡Gracias por calificar al cliente!";
            }
            catch (SqlException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
