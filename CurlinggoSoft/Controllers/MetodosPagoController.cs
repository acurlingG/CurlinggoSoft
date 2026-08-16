using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class MetodosPagoController : Controller
{
    private readonly ApplicationDbContext _context;
    public MetodosPagoController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.MetodosPago.ToListAsync());

    public async Task<IActionResult> Details(int? id) => id == null ? NotFound() : View(await _context.MetodosPago.FindAsync(id) ?? new());

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MetodoPagoID,Codigo,Nombre,Activo")] MetodoPago modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(modelo);
    }

    public async Task<IActionResult> Edit(int? id) => id == null ? NotFound() : View(await _context.MetodosPago.FindAsync(id) ?? new());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("MetodoPagoID,Codigo,Nombre,Activo")] MetodoPago modelo)
    {
        if (id != modelo.MetodoPagoID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.MetodoPagoID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        return View(modelo);
    }

    public async Task<IActionResult> Delete(int? id) => id == null ? NotFound() : View(await _context.MetodosPago.FindAsync(id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var modelo = await _context.MetodosPago.FindAsync(id);
        if (modelo != null) _context.MetodosPago.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(int? id) => _context.MetodosPago.Any(e => e.MetodoPagoID == id);
}
