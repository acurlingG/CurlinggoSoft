using CurlinggoSoft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurlinggoSoft.Controllers
{
    [Authorize(Roles = "Tecnico")]
    public class TecnicoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TecnicoController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Tecnico/Index (Dashboard del Técnico)
        public async Task<IActionResult> Index()
        {
            var tecnicoId = _userManager.GetUserId(User);

            var perfilTecnico = await _context.TecnicosPerfil
                .FirstOrDefaultAsync(t => t.TecnicoID == tecnicoId);

            ViewBag.Perfil = perfilTecnico;

            // Trabajos asignados activos
            var trabajosActivos = await _context.SolicitudesReserva
                .Include(r => r.Servicio)
                .Include(r => r.EstadoReserva)
                .Where(r => r.TecnicoID == tecnicoId)
                .OrderBy(r => r.FechaHoraProgramada)
                .ToListAsync();

            return View(trabajosActivos);
        }

        // GET: /Tecnico/OfertasDisponibles
        public async Task<IActionResult> OfertasDisponibles()
        {
            var tecnicoId = _userManager.GetUserId(User);

            // Ofertas pendientes para este técnico
            var ofertas = await _context.OfertasTecnico
                .Include(o => o.Reserva)
                    .ThenInclude(r => r.Servicio)
                .Include(o => o.EstadoOferta)
                .Where(o => o.TecnicoID == tecnicoId && o.EstadoOferta.Codigo == "PENDIENTE")
                .OrderByDescending(o => o.FechaEnvio)
                .ToListAsync();

            return View(ofertas);
        }
    }
}