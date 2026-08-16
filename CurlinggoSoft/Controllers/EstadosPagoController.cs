using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class EstadosPagoController : Controller
{
    private readonly ApplicationDbContext _context;
    public EstadosPagoController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.EstadosPago.ToListAsync());

    public async Task<IActionResult> Details(int? id) => id == null ? NotFound() : View(await _context.EstadosPago.FindAsync(id) ?? new());

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EstadoPagoID,Codigo,Nombre")] EstadoPago modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(modelo);
    }

    public async Task<IActionResult> Edit(int? id) => id == null ? NotFound() : View(await _context.EstadosPago.FindAsync(id) ?? new());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("EstadoPagoID,Codigo,Nombre")] EstadoPago modelo)
    {
        if (id != modelo.EstadoPagoID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.EstadoPagoID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        return View(modelo);
    }

    public async Task<IActionResult> Delete(int? id) => id == null ? NotFound() : View(await _context.EstadosPago.FindAsync(id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var modelo = await _context.EstadosPago.FindAsync(id);
        if (modelo != null) _context.EstadosPago.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(int? id) => _context.EstadosPago.Any(e => e.EstadoPagoID == id);
}
