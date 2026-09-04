using CurlinggoSoft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CurlinggoSoft.Controllers
{
    [Authorize(Roles = "Tecnico")]
    public partial class TecnicoController : Controller
    {
        // GET: /Tecnico/MisZonasCobertura
        [HttpGet]
        public async Task<IActionResult> MisZonasCobertura()
        {
            var tecnicoId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var zonas = await _context.TecnicoCoberturas
                .Include(z => z.Provincia)
                .Include(z => z.Canton)
                .Include(z => z.Distrito)
                .Where(z => z.TecnicoID == tecnicoId)
                .OrderByDescending(z => z.FechaCreacion)
                .ToListAsync();

            return View(zonas);
        }

        // GET: /Tecnico/AgregarZonaCobertura
        [HttpGet]
        public async Task<IActionResult> AgregarZonaCobertura()
        {
            ViewBag.Provincias = await _context.Provincias.ToListAsync();
            ViewBag.Cantones = new List<Canton>();
            ViewBag.Distritos = new List<Distrito>();

            return View();
        }

        // POST: /Tecnico/AgregarZonaCobertura
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarZonaCobertura(
            [Bind("ProvinciaID,CantonID,DistritoID,RadioCoberturaKm")] TecnicoCobertura modelo)
        {
            var tecnicoId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            modelo.TecnicoID = tecnicoId;  // ✅ Asignar TecnicoID antes de validar

            if (!ModelState.IsValid)
            {
                ViewBag.Provincias = await _context.Provincias.ToListAsync();
                ViewBag.Cantones = modelo.ProvinciaID > 0
                    ? await _context.Cantones.Where(c => c.ProvinciaID == modelo.ProvinciaID).ToListAsync()
                    : new List<Canton>();
                ViewBag.Distritos = modelo.CantonID > 0
                    ? await _context.Distritos.Where(d => d.CantonID == modelo.CantonID).ToListAsync()
                    : new List<Distrito>();

                return View(modelo);
            }

            // Verificar si ya existe esta zona
            var existe = await _context.TecnicoCoberturas.AnyAsync(z =>
                z.TecnicoID == tecnicoId &&
                z.ProvinciaID == modelo.ProvinciaID &&
                z.CantonID == modelo.CantonID &&
                z.DistritoID == modelo.DistritoID);

            if (existe)
            {
                ModelState.AddModelError("", "Ya tienes registrada esta zona de cobertura.");
                ViewBag.Provincias = await _context.Provincias.ToListAsync();
                ViewBag.Cantones = await _context.Cantones
                    .Where(c => c.ProvinciaID == modelo.ProvinciaID).ToListAsync();
                ViewBag.Distritos = modelo.DistritoID.HasValue
                    ? await _context.Distritos.Where(d => d.CantonID == modelo.CantonID).ToListAsync()
                    : new List<Distrito>();

                return View(modelo);
            }

            try
            {
                var nuevaZona = new TecnicoCobertura
                {
                    TecnicoID = tecnicoId,
                    ProvinciaID = modelo.ProvinciaID,
                    CantonID = modelo.CantonID,
                    DistritoID = modelo.DistritoID,
                    RadioCoberturaKm = modelo.RadioCoberturaKm,
                    Activa = true
                };

                _context.Add(nuevaZona);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Zona de cobertura agregada correctamente.";
                return RedirectToAction(nameof(MisZonasCobertura));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al agregar zona: {ex.Message}";

                ViewBag.Provincias = await _context.Provincias.ToListAsync();
                ViewBag.Cantones = await _context.Cantones
                    .Where(c => c.ProvinciaID == modelo.ProvinciaID).ToListAsync();
                ViewBag.Distritos = modelo.DistritoID.HasValue
                    ? await _context.Distritos.Where(d => d.CantonID == modelo.CantonID).ToListAsync()
                    : new List<Distrito>();

                return View(modelo);
            }
        }

        // GET: /Tecnico/EditarZonaCobertura/{id}
        [HttpGet]
        public async Task<IActionResult> EditarZonaCobertura(long? id)
        {
            if (id == null) return NotFound();

            var tecnicoId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var zona = await _context.TecnicoCoberturas
                .Include(z => z.Provincia)
                .Include(z => z.Canton)
                .Include(z => z.Distrito)
                .FirstOrDefaultAsync(z => z.TecnicoCoberturaID == id && z.TecnicoID == tecnicoId);

            if (zona == null)
                return Unauthorized();

            ViewBag.Provincias = await _context.Provincias.ToListAsync();
            ViewBag.Cantones = zona.ProvinciaID > 0
                ? await _context.Cantones.Where(c => c.ProvinciaID == zona.ProvinciaID).ToListAsync()
                : new List<Canton>();
            ViewBag.Distritos = zona.CantonID > 0
                ? await _context.Distritos.Where(d => d.CantonID == zona.CantonID).ToListAsync()
                : new List<Distrito>();

            return View(zona);
        }

        // POST: /Tecnico/EditarZonaCobertura/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarZonaCobertura(long? id,
            [Bind("TecnicoCoberturaID,TecnicoID,ProvinciaID,CantonID,DistritoID,RadioCoberturaKm,Activa")] TecnicoCobertura modelo)
        {
            if (id != modelo.TecnicoCoberturaID)
                return NotFound();

            var tecnicoId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            if (modelo.TecnicoID != tecnicoId)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modelo);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Zona de cobertura actualizada correctamente.";
                    return RedirectToAction(nameof(MisZonasCobertura));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.TecnicoCoberturas.Any(z => z.TecnicoCoberturaID == modelo.TecnicoCoberturaID))
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

        // POST: /Tecnico/EliminarZonaCobertura/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarZonaCobertura(long? id)
        {
            if (id == null) return NotFound();

            var tecnicoId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var zona = await _context.TecnicoCoberturas
                .FirstOrDefaultAsync(z => z.TecnicoCoberturaID == id && z.TecnicoID == tecnicoId);

            if (zona == null)
                return Unauthorized();

            try
            {
                _context.TecnicoCoberturas.Remove(zona);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Zona de cobertura eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar: {ex.Message}";
            }

            return RedirectToAction(nameof(MisZonasCobertura));
        }

        // POST: /Tecnico/DesactivarZonaCobertura/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DesactivarZonaCobertura(long? id)
        {
            if (id == null) return NotFound();

            var tecnicoId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var zona = await _context.TecnicoCoberturas
                .FirstOrDefaultAsync(z => z.TecnicoCoberturaID == id && z.TecnicoID == tecnicoId);

            if (zona == null)
                return Unauthorized();

            try
            {
                zona.Activa = false;
                _context.Update(zona);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Zona de cobertura desactivada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al desactivar: {ex.Message}";
            }

            return RedirectToAction(nameof(MisZonasCobertura));
        }

        // GET: /Tecnico/ObtenerCantones?provinciaId=X (API para dropdowns dinámicos)
        [HttpGet]
        [Authorize(Roles = "Tecnico")]
        public async Task<JsonResult> ObtenerCantones(int provinciaId)
        {
            if (provinciaId <= 0)
                return Json(new List<object>());

            var cantones = await _context.Cantones
                .Where(c => c.ProvinciaID == provinciaId)
                .OrderBy(c => c.Nombre)
                .Select(c => new { cantonID = c.CantonID, nombre = c.Nombre })
                .ToListAsync();

            return Json(cantones);
        }

        // GET: /Tecnico/ObtenerDistritos?cantonId=X (API para dropdowns dinámicos)
        [HttpGet]
        [Authorize(Roles = "Tecnico")]
        public async Task<JsonResult> ObtenerDistritos(int cantonId)
        {
            if (cantonId <= 0)
                return Json(new List<object>());

            var distritos = await _context.Distritos
                .Where(d => d.CantonID == cantonId)
                .OrderBy(d => d.Nombre)
                .Select(d => new { distritoID = d.DistritoID, nombre = d.Nombre })
                .ToListAsync();

            return Json(distritos);
        }
    }
}
