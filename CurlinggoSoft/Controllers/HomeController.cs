using CurlinggoSoft.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CurlinggoSoft.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Mismo orden que usa ServiciosController.Index: por categoría, subcategoría y nombre.
            // Solo se muestran los primeros 4 servicios activos como destacados del Home.
            var servicios = await _context.Servicios
                .Include(s => s.Categoria)
                .Include(s => s.Subcategoria)
                .Where(s => s.Activo)
                .OrderBy(s => s.Categoria.NombreCategoria)
                .ThenBy(s => s.Subcategoria.NombreSubcategoria)
                .ThenBy(s => s.NombreServicio)
                .Take(4)
                .ToListAsync();

            return View(servicios);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Nosotros()
        {
            return View();
        }

        public IActionResult ComoFunciona()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}