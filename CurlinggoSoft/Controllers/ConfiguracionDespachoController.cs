using CurlinggoSoft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurlinggoSoft.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConfiguracionDespachoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConfiguracionDespachoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ConfiguracionDespacho/Edit
        public async Task<IActionResult> Edit()
        {
            var config = await _context.ConfiguracionDespacho.FindAsync(1);
            if (config == null)
            {
                config = new ConfiguracionDespacho
                {
                    ConfiguracionDespachoID = 1,
                    RadioKm = 20.00m,
                    MaxTecnicos = 10,
                    FechaActualizacion = DateTime.Now
                };
                _context.ConfiguracionDespacho.Add(config);
                await _context.SaveChangesAsync();
            }

            return View(config);
        }

        // POST: ConfiguracionDespacho/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ConfiguracionDespacho model)
        {
            if (model.ConfiguracionDespachoID != 1)
            {
                model.ConfiguracionDespachoID = 1;
            }

            if (model.RadioKm <= 0)
            {
                ModelState.AddModelError(nameof(model.RadioKm), "El radio en km debe ser mayor a 0.");
            }

            if (model.MaxTecnicos <= 0)
            {
                ModelState.AddModelError(nameof(model.MaxTecnicos), "El máximo de técnicos debe ser mayor a 0.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var config = await _context.ConfiguracionDespacho.FindAsync(1);
            if (config == null)
            {
                config = new ConfiguracionDespacho { ConfiguracionDespachoID = 1 };
                _context.ConfiguracionDespacho.Add(config);
            }

            config.RadioKm = model.RadioKm;
            config.MaxTecnicos = model.MaxTecnicos;
            config.FechaActualizacion = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Configuración de despacho actualizada correctamente.";
            return RedirectToAction(nameof(Edit));
        }
    }
}
