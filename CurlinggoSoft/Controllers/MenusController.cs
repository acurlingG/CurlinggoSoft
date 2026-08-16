using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class MenusController : Controller
{
    private readonly ApplicationDbContext _context;
    public MenusController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Menus.Include(m => m.MenuPadre).OrderBy(m => m.Orden).ToListAsync());

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.Menus.Include(m => m.MenuPadre).FirstOrDefaultAsync(m => m.MenuID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["MenuPadreID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Menus, "MenuID", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MenuID,MenuPadreID,Nombre,Url,Icono,Orden,Activo")] Menu modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["MenuPadreID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Menus, "MenuID", "Nombre", modelo.MenuPadreID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.Menus.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["MenuPadreID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Menus, "MenuID", "Nombre", modelo.MenuPadreID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("MenuID,MenuPadreID,Nombre,Url,Icono,Orden,Activo")] Menu modelo)
    {
        if (id != modelo.MenuID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.MenuID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["MenuPadreID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Menus, "MenuID", "Nombre", modelo.MenuPadreID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.Menus.Include(m => m.MenuPadre).FirstOrDefaultAsync(m => m.MenuID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.Menus.FindAsync(id);
        if (modelo != null) _context.Menus.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.Menus.Any(e => e.MenuID == id);
}
