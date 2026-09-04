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
                .OrderByDescending(r => r.ReservaID)
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
                .OrderByDescending(r => r.ReservaID)
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

        // GET: /Cliente/EditarDireccion/{id}
        [HttpGet]
        public async Task<IActionResult> EditarDireccion(long? id)
        {
            if (id == null) return NotFound();

            var clienteId = _userManager.GetUserId(User);
            var direccion = await _context.DireccionesCliente
                .Include(d => d.Provincia)
                .Include(d => d.Canton)
                .Include(d => d.Distrito)
                .FirstOrDefaultAsync(d => d.DireccionID == id && d.ClienteID == clienteId);

            if (direccion == null)
                return Unauthorized();

            // Cargar datos para dropdowns
            ViewBag.Provincias = await _context.Provincias.ToListAsync();
            ViewBag.Cantones = direccion.ProvinciaID > 0 
                ? await _context.Cantones.Where(c => c.ProvinciaID == direccion.ProvinciaID).ToListAsync()
                : new List<Canton>();
            ViewBag.Distritos = direccion.CantonID > 0
                ? await _context.Distritos.Where(d => d.CantonID == direccion.CantonID).ToListAsync()
                : new List<Distrito>();

            return View(direccion);
        }

        // POST: /Cliente/EditarDireccion/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarDireccion(long? id, 
            [Bind("DireccionID,ClienteID,NombreDireccion,ProvinciaID,CantonID,DistritoID,DireccionExacta,Activa")] DireccionCliente modelo)
        {
            if (id != modelo.DireccionID)
                return NotFound();

            var clienteId = _userManager.GetUserId(User);
            if (modelo.ClienteID != clienteId)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modelo);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Dirección actualizada correctamente.";
                    return RedirectToAction(nameof(MisDirecciones));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.DireccionesCliente.Any(d => d.DireccionID == modelo.DireccionID))
                        return NotFound();
                    throw;
                }
            }

            ViewBag.Provincias = await _context.Provincias.ToListAsync();
            ViewBag.Cantones = modelo.ProvinciaID > 0
                ? await _context.Cantones.Where(c => c.ProvinciaID == modelo.ProvinciaID).ToListAsync()
                : new List<Canton>();
            ViewBag.Distritos = modelo.CantonID > 0
                ? await _context.Distritos.Where(d => d.CantonID == modelo.CantonID).ToListAsync()
                : new List<Distrito>();

            return View(modelo);
        }

        // POST: /Cliente/EliminarDireccion/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarDireccion(long? id)
        {
            if (id == null) return NotFound();

            var clienteId = _userManager.GetUserId(User);
            var direccion = await _context.DireccionesCliente
                .FirstOrDefaultAsync(d => d.DireccionID == id && d.ClienteID == clienteId);

            if (direccion == null)
                return Unauthorized();

            try
            {
                _context.DireccionesCliente.Remove(direccion);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Dirección eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar: {ex.Message}";
            }

            return RedirectToAction(nameof(MisDirecciones));
        }

        // POST: /Cliente/DeshabilitarDireccion/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeshabilitarDireccion(long? id)
        {
            if (id == null) return NotFound();

            var clienteId = _userManager.GetUserId(User);
            var direccion = await _context.DireccionesCliente
                .FirstOrDefaultAsync(d => d.DireccionID == id && d.ClienteID == clienteId);

            if (direccion == null)
                return Unauthorized();

            try
            {
                direccion.Activa = false;
                _context.Update(direccion);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Dirección deshabilitada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al deshabilitar: {ex.Message}";
            }

            return RedirectToAction(nameof(MisDirecciones));
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