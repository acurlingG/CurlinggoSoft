using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class TiposEvaluacionController : Controller
{
    private readonly ApplicationDbContext _context;
    public TiposEvaluacionController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.TiposEvaluacion.ToListAsync());

    public async Task<IActionResult> Details(int? id) => id == null ? NotFound() : View(await _context.TiposEvaluacion.FindAsync(id) ?? new());

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TipoEvaluacionID,Codigo,Nombre")] TipoEvaluacion modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(modelo);
    }

    public async Task<IActionResult> Edit(int? id) => id == null ? NotFound() : View(await _context.TiposEvaluacion.FindAsync(id) ?? new());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("TipoEvaluacionID,Codigo,Nombre")] TipoEvaluacion modelo)
    {
        if (id != modelo.TipoEvaluacionID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.TipoEvaluacionID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        return View(modelo);
    }

    public async Task<IActionResult> Delete(int? id) => id == null ? NotFound() : View(await _context.TiposEvaluacion.FindAsync(id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var modelo = await _context.TiposEvaluacion.FindAsync(id);
        if (modelo != null) _context.TiposEvaluacion.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(int? id) => _context.TiposEvaluacion.Any(e => e.TipoEvaluacionID == id);
}
