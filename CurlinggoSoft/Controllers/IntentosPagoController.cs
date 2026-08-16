using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class IntentosPagoController : Controller
{
    private readonly ApplicationDbContext _context;
    public IntentosPagoController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.IntentosPago.Include(i => i.Pago).Include(i => i.MetodoPago).Include(i => i.EstadoPago).OrderByDescending(i => i.FechaIntento).ToListAsync());

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.IntentosPago.Include(i => i.Pago).Include(i => i.MetodoPago).Include(i => i.EstadoPago).FirstOrDefaultAsync(m => m.IntentoPagoID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["PagoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Pagos, "PagoID", "PagoID");
        ViewData["MetodoPagoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.MetodosPago, "MetodoPagoID", "Nombre");
        ViewData["EstadoPagoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosPago, "EstadoPagoID", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IntentoPagoID,PagoID,MetodoPagoID,EstadoPagoID,MontoIntento,ReferenciaComprobante,ReferenciaProveedor,FechaIntento,MensajeProveedor")] IntentoPago modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["PagoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Pagos, "PagoID", "PagoID", modelo.PagoID);
        ViewData["MetodoPagoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.MetodosPago, "MetodoPagoID", "Nombre", modelo.MetodoPagoID);
        ViewData["EstadoPagoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosPago, "EstadoPagoID", "Nombre", modelo.EstadoPagoID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.IntentosPago.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["PagoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Pagos, "PagoID", "PagoID", modelo.PagoID);
        ViewData["MetodoPagoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.MetodosPago, "MetodoPagoID", "Nombre", modelo.MetodoPagoID);
        ViewData["EstadoPagoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosPago, "EstadoPagoID", "Nombre", modelo.EstadoPagoID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("IntentoPagoID,PagoID,MetodoPagoID,EstadoPagoID,MontoIntento,ReferenciaComprobante,ReferenciaProveedor,FechaIntento,MensajeProveedor")] IntentoPago modelo)
    {
        if (id != modelo.IntentoPagoID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.IntentoPagoID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["PagoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Pagos, "PagoID", "PagoID", modelo.PagoID);
        ViewData["MetodoPagoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.MetodosPago, "MetodoPagoID", "Nombre", modelo.MetodoPagoID);
        ViewData["EstadoPagoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosPago, "EstadoPagoID", "Nombre", modelo.EstadoPagoID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.IntentosPago.Include(i => i.Pago).Include(i => i.MetodoPago).Include(i => i.EstadoPago).FirstOrDefaultAsync(m => m.IntentoPagoID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.IntentosPago.FindAsync(id);
        if (modelo != null) _context.IntentosPago.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.IntentosPago.Any(e => e.IntentoPagoID == id);
}
