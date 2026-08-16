using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class SubcategoriasServicioController : Controller
{
    private readonly ApplicationDbContext _context;
    public SubcategoriasServicioController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var items = await _context.SubcategoriasServicio.Include(s => s.Categoria).OrderBy(s => s.Categoria.NombreCategoria).ThenBy(s => s.NombreSubcategoria).ToListAsync();
        return View(items);
    }

    public async Task<IActionResult> Details(int? id) => id == null ? NotFound() : View(await _context.SubcategoriasServicio.Include(s => s.Categoria).FirstOrDefaultAsync(m => m.SubcategoriaID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["CategoriaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.CategoriasServicio.OrderBy(c => c.NombreCategoria), "CategoriaID", "NombreCategoria");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("SubcategoriaID,CategoriaID,NombreSubcategoria,Descripcion,Activa")] SubcategoriaServicio modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoriaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.CategoriasServicio, "CategoriaID", "NombreCategoria", modelo.CategoriaID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.SubcategoriasServicio.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["CategoriaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.CategoriasServicio, "CategoriaID", "NombreCategoria", modelo.CategoriaID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("SubcategoriaID,CategoriaID,NombreSubcategoria,Descripcion,Activa")] SubcategoriaServicio modelo)
    {
        if (id != modelo.SubcategoriaID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.SubcategoriaID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoriaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.CategoriasServicio, "CategoriaID", "NombreCategoria", modelo.CategoriaID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(int? id) => id == null ? NotFound() : View(await _context.SubcategoriasServicio.Include(s => s.Categoria).FirstOrDefaultAsync(m => m.SubcategoriaID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var modelo = await _context.SubcategoriasServicio.FindAsync(id);
        if (modelo != null) _context.SubcategoriasServicio.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(int? id) => _context.SubcategoriasServicio.Any(e => e.SubcategoriaID == id);
}
