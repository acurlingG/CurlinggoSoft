using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class PagosController : Controller
{
    private readonly ApplicationDbContext _context;
    public PagosController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Pagos.Include(p => p.Reserva).OrderByDescending(p => p.FechaCreacion).ToListAsync());

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.Pagos.Include(p => p.Reserva).FirstOrDefaultAsync(m => m.PagoID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PagoID,ReservaID,MontoTotal,ComisionPlataforma,MontoNetoTecnico,Moneda,ProveedorPago,IdempotencyKey,FechaCreacion")] Pago modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.Pagos.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("PagoID,ReservaID,MontoTotal,ComisionPlataforma,MontoNetoTecnico,Moneda,ProveedorPago,IdempotencyKey,FechaCreacion")] Pago modelo)
    {
        if (id != modelo.PagoID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.PagoID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.Pagos.Include(p => p.Reserva).FirstOrDefaultAsync(m => m.PagoID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.Pagos.FindAsync(id);
        if (modelo != null) _context.Pagos.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.Pagos.Any(e => e.PagoID == id);
}
