using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class DistritosController : Controller
{
    private readonly ApplicationDbContext _context;

    public DistritosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var distritos = await _context.Distritos
            .Include(d => d.Canton)
            .ThenInclude(c => c.Provincia)
            .OrderBy(d => d.Canton.Provincia.Nombre)
            .ThenBy(d => d.Canton.Nombre)
            .ThenBy(d => d.Nombre)
            .ToListAsync();
        return View(distritos);
    }

    public async Task<IActionResult> Details(int? distritoid)
    {
        if (distritoid == null)
            return NotFound();

        var distrito = await _context.Distritos
            .Include(d => d.Canton)
            .ThenInclude(c => c.Provincia)
            .FirstOrDefaultAsync(d => d.DistritoID == distritoid);

        return distrito == null ? NotFound() : View(distrito);
    }

    public IActionResult Create()
    {
        ViewData["CantonID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones.Include(c => c.Provincia).OrderBy(c => c.Provincia.Nombre).ThenBy(c => c.Nombre), "CantonID", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DistritoID,CantonID,Nombre,CodigoDTA,Activo")] Distrito distrito)
    {
        if (ModelState.IsValid)
        {
            _context.Add(distrito);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CantonID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones.Include(c => c.Provincia), "CantonID", "Nombre", distrito.CantonID);
        return View(distrito);
    }

    public async Task<IActionResult> Edit(int? distritoid)
    {
        if (distritoid == null)
            return NotFound();

        var distrito = await _context.Distritos.FindAsync(distritoid);
        if (distrito == null)
            return NotFound();

        ViewData["CantonID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones.Include(c => c.Provincia), "CantonID", "Nombre", distrito.CantonID);
        return View(distrito);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? distritoid, [Bind("DistritoID,CantonID,Nombre,CodigoDTA,Activo")] Distrito distrito)
    {
        if (distritoid != distrito.DistritoID)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(distrito);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistritoExists(distrito.DistritoID))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["CantonID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones.Include(c => c.Provincia), "CantonID", "Nombre", distrito.CantonID);
        return View(distrito);
    }

    public async Task<IActionResult> Delete(int? distritoid)
    {
        if (distritoid == null)
            return NotFound();

        var distrito = await _context.Distritos
            .Include(d => d.Canton)
            .ThenInclude(c => c.Provincia)
            .FirstOrDefaultAsync(d => d.DistritoID == distritoid);

        return distrito == null ? NotFound() : View(distrito);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? distritoid)
    {
        var distrito = await _context.Distritos.FindAsync(distritoid);
        if (distrito != null)
            _context.Distritos.Remove(distrito);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DistritoExists(int? distritoid)
    {
        return _context.Distritos.Any(e => e.DistritoID == distritoid);
    }
}
