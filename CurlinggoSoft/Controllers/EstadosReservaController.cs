using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class EstadosReservaController : Controller
{
    private readonly ApplicationDbContext _context;
    public EstadosReservaController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.EstadosReserva.OrderBy(e => e.OrdenFlujo).ToListAsync());

    public async Task<IActionResult> Details(int? id) => id == null ? NotFound() : View(await _context.EstadosReserva.FindAsync(id) ?? new());

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EstadoReservaID,Codigo,Nombre,OrdenFlujo")] EstadoReserva modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(modelo);
    }

    public async Task<IActionResult> Edit(int? id) => id == null ? NotFound() : View(await _context.EstadosReserva.FindAsync(id) ?? new());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("EstadoReservaID,Codigo,Nombre,OrdenFlujo")] EstadoReserva modelo)
    {
        if (id != modelo.EstadoReservaID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.EstadoReservaID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        return View(modelo);
    }

    public async Task<IActionResult> Delete(int? id) => id == null ? NotFound() : View(await _context.EstadosReserva.FindAsync(id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var modelo = await _context.EstadosReserva.FindAsync(id);
        if (modelo != null) _context.EstadosReserva.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(int? id) => _context.EstadosReserva.Any(e => e.EstadoReservaID == id);
}
