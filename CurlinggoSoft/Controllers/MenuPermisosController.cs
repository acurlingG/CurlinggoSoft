using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class MenuPermisosController : Controller
{
    private readonly ApplicationDbContext _context;
    public MenuPermisosController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.MenuPermisos.Include(mp => mp.Menu).Include(mp => mp.Permiso).ToListAsync());

    public async Task<IActionResult> Details(long? menuId, int? permisoId) => (menuId == null || permisoId == null) ? NotFound() : View(await _context.MenuPermisos.Include(mp => mp.Menu).Include(mp => mp.Permiso).FirstOrDefaultAsync(m => m.MenuID == menuId && m.PermisoID == permisoId) ?? new());

    public IActionResult Create()
    {
        ViewData["MenuID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Menus, "MenuID", "Nombre");
        ViewData["PermisoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Permisos, "PermisoID", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MenuID,PermisoID")] MenuPermiso modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["MenuID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Menus, "MenuID", "Nombre", modelo.MenuID);
        ViewData["PermisoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Permisos, "PermisoID", "Nombre", modelo.PermisoID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? menuId, int? permisoId) => (menuId == null || permisoId == null) ? NotFound() : View(await _context.MenuPermisos.Include(mp => mp.Menu).Include(mp => mp.Permiso).FirstOrDefaultAsync(m => m.MenuID == menuId && m.PermisoID == permisoId) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long menuId, int permisoId)
    {
        var modelo = await _context.MenuPermisos.FindAsync(menuId, permisoId);
        if (modelo != null) _context.MenuPermisos.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
