using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class HistorialEstadosReservaController : Controller
{
    private readonly ApplicationDbContext _context;
    public HistorialEstadosReservaController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.HistorialEstadosReserva.Include(h => h.Reserva).Include(h => h.EstadoAnterior).Include(h => h.EstadoNuevo).OrderByDescending(h => h.FechaCambio).ToListAsync());

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.HistorialEstadosReserva.Include(h => h.Reserva).Include(h => h.EstadoAnterior).Include(h => h.EstadoNuevo).Include(h => h.UsuarioModificador).FirstOrDefaultAsync(m => m.HistorialID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio");
        ViewData["EstadoAnteriorID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosReserva, "EstadoReservaID", "Nombre");
        ViewData["EstadoNuevoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosReserva, "EstadoReservaID", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("HistorialID,ReservaID,EstadoAnteriorID,EstadoNuevoID,FechaCambio,UsuarioModificadorID,Observaciones")] HistorialEstadoReserva modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["EstadoAnteriorID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosReserva, "EstadoReservaID", "Nombre", modelo.EstadoAnteriorID);
        ViewData["EstadoNuevoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosReserva, "EstadoReservaID", "Nombre", modelo.EstadoNuevoID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.HistorialEstadosReserva.Include(h => h.Reserva).Include(h => h.EstadoAnterior).Include(h => h.EstadoNuevo).FirstOrDefaultAsync(m => m.HistorialID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.HistorialEstadosReserva.FindAsync(id);
        if (modelo != null) _context.HistorialEstadosReserva.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
