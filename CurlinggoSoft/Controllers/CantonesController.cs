using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class CantonesController : Controller
{
    private readonly ApplicationDbContext _context;

    public CantonesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: CANTONES/PorProvincia/5
    // Usado por los formularios de Perfiles de Cliente/Técnico para llenar
    // el combo de Cantón en cascada según la Provincia seleccionada.
    [HttpGet]
    public async Task<IActionResult> PorProvincia(int? provinciaId)
    {
        if (provinciaId == null)
        {
            return Json(Array.Empty<object>());
        }

        var cantones = await _context.Cantones
            .Where(c => c.ProvinciaID == provinciaId && c.Activo)
            .OrderBy(c => c.Nombre)
            .Select(c => new { c.CantonID, c.Nombre })
            .ToListAsync();

        return Json(cantones);
    }

    // GET: CANTONES
    public async Task<IActionResult> Index()
    {
        var cantones = await _context.Cantones
            .Include(c => c.Provincia)
            .OrderBy(c => c.Provincia.Nombre)
            .ThenBy(c => c.Nombre)
            .ToListAsync();
        return View(cantones);
    }

    // GET: CANTONES/Details/5
    public async Task<IActionResult> Details(int? cantonid)
    {
        if (cantonid == null)
        {
            return NotFound();
        }

        var canton = await _context.Cantones
            .Include(c => c.Provincia)
            .FirstOrDefaultAsync(m => m.CantonID == cantonid);
        if (canton == null)
        {
            return NotFound();
        }

        return View(canton);
    }

    // GET: CANTONES/Create
    public IActionResult Create()
    {
        ViewData["ProvinciaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre");
        return View();
    }

    // POST: CANTONES/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CantonID,ProvinciaID,Nombre,CodigoDTA,Activo")] Canton canton)
    {
        if (ModelState.IsValid)
        {
            _context.Add(canton);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ProvinciaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre", canton.ProvinciaID);
        return View(canton);
    }

    // GET: CANTONES/Edit/5
    public async Task<IActionResult> Edit(int? cantonid)
    {
        if (cantonid == null)
        {
            return NotFound();
        }

        var canton = await _context.Cantones.FindAsync(cantonid);
        if (canton == null)
        {
            return NotFound();
        }
        ViewData["ProvinciaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre", canton.ProvinciaID);
        return View(canton);
    }

    // POST: CANTONES/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? cantonid, [Bind("CantonID,ProvinciaID,Nombre,CodigoDTA,Activo")] Canton canton)
    {
        if (cantonid != canton.CantonID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(canton);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CantonExists(canton.CantonID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ProvinciaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre", canton.ProvinciaID);
        return View(canton);
    }

    // GET: CANTONES/Delete/5
    public async Task<IActionResult> Delete(int? cantonid)
    {
        if (cantonid == null)
        {
            return NotFound();
        }

        var canton = await _context.Cantones
            .Include(c => c.Provincia)
            .FirstOrDefaultAsync(m => m.CantonID == cantonid);
        if (canton == null)
        {
            return NotFound();
        }

        return View(canton);
    }

    // POST: CANTONES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? cantonid)
    {
        var canton = await _context.Cantones.FindAsync(cantonid);
        if (canton != null)
        {
            _context.Cantones.Remove(canton);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CantonExists(int? cantonid)
    {
        return _context.Cantones.Any(e => e.CantonID == cantonid);
    }
}
