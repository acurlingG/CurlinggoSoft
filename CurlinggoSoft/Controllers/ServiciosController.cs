using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class ServiciosController : Controller
{
    private readonly ApplicationDbContext _context;
    public ServiciosController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var items = await _context.Servicios.Include(s => s.Categoria).Include(s => s.Subcategoria).OrderBy(s => s.Categoria.NombreCategoria).ThenBy(s => s.Subcategoria.NombreSubcategoria).ThenBy(s => s.NombreServicio).ToListAsync();
        return View(items);
    }

    public async Task<IActionResult> Details(int? id) => id == null ? NotFound() : View(await _context.Servicios.Include(s => s.Categoria).Include(s => s.Subcategoria).FirstOrDefaultAsync(m => m.ServicioID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["CategoriaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.CategoriasServicio.OrderBy(c => c.NombreCategoria), "CategoriaID", "NombreCategoria");
        ViewData["SubcategoriaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SubcategoriasServicio.OrderBy(s => s.NombreSubcategoria), "SubcategoriaID", "NombreSubcategoria");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ServicioID,CategoriaID,SubcategoriaID,NombreServicio,Descripcion,TarifaDiagnosticoBase,TiempoEstimadoMinutos,Moneda,Activo")] Servicio modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoriaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.CategoriasServicio, "CategoriaID", "NombreCategoria", modelo.CategoriaID);
        ViewData["SubcategoriaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SubcategoriasServicio, "SubcategoriaID", "NombreSubcategoria", modelo.SubcategoriaID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.Servicios.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["CategoriaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.CategoriasServicio, "CategoriaID", "NombreCategoria", modelo.CategoriaID);
        ViewData["SubcategoriaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SubcategoriasServicio, "SubcategoriaID", "NombreSubcategoria", modelo.SubcategoriaID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("ServicioID,CategoriaID,SubcategoriaID,NombreServicio,Descripcion,TarifaDiagnosticoBase,TiempoEstimadoMinutos,Moneda,Activo")] Servicio modelo)
    {
        if (id != modelo.ServicioID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.ServicioID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoriaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.CategoriasServicio, "CategoriaID", "NombreCategoria", modelo.CategoriaID);
        ViewData["SubcategoriaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SubcategoriasServicio, "SubcategoriaID", "NombreSubcategoria", modelo.SubcategoriaID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(int? id) => id == null ? NotFound() : View(await _context.Servicios.Include(s => s.Categoria).Include(s => s.Subcategoria).FirstOrDefaultAsync(m => m.ServicioID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var modelo = await _context.Servicios.FindAsync(id);
        if (modelo != null) _context.Servicios.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(int? id) => _context.Servicios.Any(e => e.ServicioID == id);
}
