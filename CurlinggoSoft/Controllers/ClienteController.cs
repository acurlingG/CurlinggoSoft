using CurlinggoSoft.Models;
using CurlinggoSoft.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CurlinggoSoft.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class ClienteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly EvaluacionService _evaluacionService;

        public ClienteController(ApplicationDbContext context, UserManager<IdentityUser> userManager, EvaluacionService evaluacionService)
        {
            _context = context;
            _userManager = userManager;
            _evaluacionService = evaluacionService;
        }

        // GET: /Cliente/Index (Dashboard del Cliente)
        public async Task<IActionResult> Index()
        {
            var clienteId = _userManager.GetUserId(User);

            // Estadísticas rápidas o reservas recientes del cliente
            var reservasRecientes = await _context.SolicitudesReserva
                .Include(r => r.Servicio)
                .Include(r => r.EstadoReserva)
                .Where(r => r.ClienteID == clienteId)
                .OrderByDescending(r => r.FechaHoraSolicitud)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalReservas = await _context.SolicitudesReserva.CountAsync(r => r.ClienteID == clienteId);

            return View(reservasRecientes);
        }

        // GET: /Cliente/MisReservas
        public async Task<IActionResult> MisReservas()
        {
            var clienteId = _userManager.GetUserId(User);
            var reservas = await _context.SolicitudesReserva
                .Include(r => r.Servicio)
                .Include(r => r.EstadoReserva)
                .Include(r => r.Tecnico)
                .Where(r => r.ClienteID == clienteId)
                .OrderByDescending(r => r.FechaHoraSolicitud)
                .ToListAsync();

            return View(reservas);
        }

        // GET: /Cliente/MisDirecciones
        public async Task<IActionResult> MisDirecciones()
        {
            var clienteId = _userManager.GetUserId(User);
            var direcciones = await _context.DireccionesCliente
                .Include(d => d.Provincia)
                .Include(d => d.Canton)
                .Include(d => d.Distrito)
                .Where(d => d.ClienteID == clienteId && d.Activa)
                .ToListAsync();

            return View(direcciones);
        }

        // GET: /Cliente/CalificarTecnico?reservaId=123
        // Se muestra en MisReservas.cshtml cuando la reserva está COMPLETADO.
        [HttpGet]
        public async Task<IActionResult> CalificarTecnico(long reservaId)
        {
            var clienteId = _userManager.GetUserId(User);
            var reserva = await _context.SolicitudesReserva
                .Include(r => r.EstadoReserva)
                .FirstOrDefaultAsync(r => r.ReservaID == reservaId && r.ClienteID == clienteId);

            if (reserva is null || reserva.EstadoReserva?.Codigo != "COMPLETADA" || reserva.TecnicoID is null)
                return NotFound();

            var yaCalifico = await _context.Evaluaciones.AnyAsync(e =>
                e.ReservaID == reservaId &&
                e.EvaluadorUsuarioID == clienteId &&
                e.TipoEvaluacion!.Codigo == "CLIENTE_A_TECNICO");

            ViewData["ReservaID"] = reservaId;
            ViewData["YaCalifico"] = yaCalifico;
            ViewData["TituloCalificacion"] = "¿Cómo te fue con el técnico?";
            ViewData["AccionEnviar"] = nameof(CalificarTecnico);
            return PartialView("~/Views/Shared/_CalificacionPartial.cshtml");
        }

        // POST: /Cliente/CalificarTecnico
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalificarTecnico(long reservaId, byte puntuacion, string? comentario)
        {
            var clienteId = _userManager.GetUserId(User)!;
            var reserva = await _context.SolicitudesReserva
                .Include(r => r.EstadoReserva)
                .FirstOrDefaultAsync(r => r.ReservaID == reservaId && r.ClienteID == clienteId);

            if (reserva is null || reserva.EstadoReserva?.Codigo != "COMPLETADA" || reserva.TecnicoID is null)
                return NotFound();

            try
            {
                var tipoId = await _context.TiposEvaluacion
                    .Where(t => t.Codigo == "CLIENTE_A_TECNICO")
                    .Select(t => t.TipoEvaluacionID)
                    .FirstOrDefaultAsync();

                await _evaluacionService.CrearAsync(reservaId, clienteId, reserva.TecnicoID,
                    reserva.ServicioID, tipoId, puntuacion, comentario);

                TempData["Success"] = "¡Gracias por tu calificación!";
            }
            catch (SqlException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(MisReservas));
        }
    }
}