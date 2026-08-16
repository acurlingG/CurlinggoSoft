using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class PermisosController : Controller
{
    private readonly ApplicationDbContext _context;
    public PermisosController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Permisos.OrderBy(p => p.Nombre).ToListAsync());

    public async Task<IActionResult> Details(int? id) => id == null ? NotFound() : View(await _context.Permisos.FirstOrDefaultAsync(m => m.PermisoID == id) ?? new());

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PermisoID,CodigoPermiso,Nombre,Descripcion,Activo")] Permiso modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(modelo);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.Permisos.FindAsync(id);
        if (modelo == null) return NotFound();
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("PermisoID,CodigoPermiso,Nombre,Descripcion,Activo")] Permiso modelo)
    {
        if (id != modelo.PermisoID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.PermisoID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        return View(modelo);
    }

    public async Task<IActionResult> Delete(int? id) => id == null ? NotFound() : View(await _context.Permisos.FirstOrDefaultAsync(m => m.PermisoID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var modelo = await _context.Permisos.FindAsync(id);
        if (modelo != null) _context.Permisos.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(int? id) => _context.Permisos.Any(e => e.PermisoID == id);
}
