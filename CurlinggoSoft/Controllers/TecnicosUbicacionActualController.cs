using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class TecnicosUbicacionActualController : Controller
{
    private readonly ApplicationDbContext _context;
    public TecnicosUbicacionActualController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.TecnicosUbicacionActual.Include(t => t.Tecnico).OrderByDescending(t => t.FechaActualizacion).ToListAsync());

    public async Task<IActionResult> Details(string? id) => id == null ? NotFound() : View(await _context.TecnicosUbicacionActual.Include(t => t.Tecnico).FirstOrDefaultAsync(m => m.TecnicoID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TecnicoID,Latitud,Longitud,FechaActualizacion")] TecnicoUbicacionActual modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.TecnicosUbicacionActual.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? id, [Bind("TecnicoID,Latitud,Longitud,FechaActualizacion")] TecnicoUbicacionActual modelo)
    {
        if (id != modelo.TecnicoID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.TecnicoID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(string? id) => id == null ? NotFound() : View(await _context.TecnicosUbicacionActual.Include(t => t.Tecnico).FirstOrDefaultAsync(m => m.TecnicoID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? id)
    {
        var modelo = await _context.TecnicosUbicacionActual.FindAsync(id);
        if (modelo != null) _context.TecnicosUbicacionActual.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(string? id) => _context.TecnicosUbicacionActual.Any(e => e.TecnicoID == id);
}
