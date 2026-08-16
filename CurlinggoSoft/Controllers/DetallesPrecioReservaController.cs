using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class DetallesPrecioReservaController : Controller
{
    private readonly ApplicationDbContext _context;
    public DetallesPrecioReservaController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.DetallesPrecioReserva.Include(d => d.Reserva).Include(d => d.Opcion).OrderByDescending(d => d.FechaRegistro).ToListAsync());

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.DetallesPrecioReserva.Include(d => d.Reserva).Include(d => d.Opcion).FirstOrDefaultAsync(m => m.DetallePrecioReservaID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio");
        ViewData["OpcionPreguntaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OpcionesPregunta, "OpcionPreguntaID", "TextoOpcion");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DetallePrecioReservaID,ReservaID,Concepto,Monto,OpcionPreguntaID,FechaRegistro")] DetallePrecioReserva modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["OpcionPreguntaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OpcionesPregunta, "OpcionPreguntaID", "TextoOpcion", modelo.OpcionPreguntaID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.DetallesPrecioReserva.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["OpcionPreguntaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OpcionesPregunta, "OpcionPreguntaID", "TextoOpcion", modelo.OpcionPreguntaID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("DetallePrecioReservaID,ReservaID,Concepto,Monto,OpcionPreguntaID,FechaRegistro")] DetallePrecioReserva modelo)
    {
        if (id != modelo.DetallePrecioReservaID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.DetallePrecioReservaID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["OpcionPreguntaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OpcionesPregunta, "OpcionPreguntaID", "TextoOpcion", modelo.OpcionPreguntaID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.DetallesPrecioReserva.Include(d => d.Reserva).Include(d => d.Opcion).FirstOrDefaultAsync(m => m.DetallePrecioReservaID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.DetallesPrecioReserva.FindAsync(id);
        if (modelo != null) _context.DetallesPrecioReserva.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.DetallesPrecioReserva.Any(e => e.DetallePrecioReservaID == id);
}
