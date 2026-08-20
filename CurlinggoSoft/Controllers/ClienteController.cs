using CurlinggoSoft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurlinggoSoft.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class ClienteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ClienteController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
    }
}