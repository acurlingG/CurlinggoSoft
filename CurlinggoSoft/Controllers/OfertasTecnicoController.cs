using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class OfertasTecnicoController : Controller
{
    private readonly ApplicationDbContext _context;
    public OfertasTecnicoController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.OfertasTecnico.Include(o => o.Reserva).Include(o => o.Tecnico).Include(o => o.EstadoOferta).OrderByDescending(o => o.FechaEnvio).ToListAsync());

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.OfertasTecnico.Include(o => o.Reserva).Include(o => o.Tecnico).Include(o => o.EstadoOferta).FirstOrDefaultAsync(m => m.OfertaTecnicoID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio");
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula");
        ViewData["EstadoOfertaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosOfertaTecnico, "EstadoOfertaID", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("OfertaTecnicoID,ReservaID,TecnicoID,EstadoOfertaID,DistanciaMetros,OrdenOferta,FechaEnvio,FechaExpiracion,FechaRespuesta,Mensaje")] OfertaTecnico modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        ViewData["EstadoOfertaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosOfertaTecnico, "EstadoOfertaID", "Nombre", modelo.EstadoOfertaID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.OfertasTecnico.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        ViewData["EstadoOfertaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosOfertaTecnico, "EstadoOfertaID", "Nombre", modelo.EstadoOfertaID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("OfertaTecnicoID,ReservaID,TecnicoID,EstadoOfertaID,DistanciaMetros,OrdenOferta,FechaEnvio,FechaExpiracion,FechaRespuesta,Mensaje")] OfertaTecnico modelo)
    {
        if (id != modelo.OfertaTecnicoID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.OfertaTecnicoID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        ViewData["EstadoOfertaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosOfertaTecnico, "EstadoOfertaID", "Nombre", modelo.EstadoOfertaID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.OfertasTecnico.Include(o => o.Reserva).Include(o => o.Tecnico).Include(o => o.EstadoOferta).FirstOrDefaultAsync(m => m.OfertaTecnicoID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.OfertasTecnico.FindAsync(id);
        if (modelo != null) _context.OfertasTecnico.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.OfertasTecnico.Any(e => e.OfertaTecnicoID == id);
}
