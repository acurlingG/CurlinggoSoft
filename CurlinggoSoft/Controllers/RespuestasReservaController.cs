using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class RespuestasReservaController : Controller
{
    private readonly ApplicationDbContext _context;
    public RespuestasReservaController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.RespuestasReserva.Include(r => r.Reserva).Include(r => r.Pregunta).Include(r => r.Opcion).OrderByDescending(r => r.FechaRespuesta).ToListAsync());

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.RespuestasReserva.Include(r => r.Reserva).Include(r => r.Pregunta).Include(r => r.Opcion).FirstOrDefaultAsync(m => m.RespuestaReservaID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio");
        ViewData["PreguntaServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.PreguntasServicio, "PreguntaServicioID", "TextoPregunta");
        ViewData["OpcionPreguntaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OpcionesPregunta, "OpcionPreguntaID", "TextoOpcion");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("RespuestaReservaID,ReservaID,PreguntaServicioID,OpcionPreguntaID,RespuestaTexto,FechaRespuesta")] RespuestaReserva modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["PreguntaServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.PreguntasServicio, "PreguntaServicioID", "TextoPregunta", modelo.PreguntaServicioID);
        ViewData["OpcionPreguntaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OpcionesPregunta, "OpcionPreguntaID", "TextoOpcion", modelo.OpcionPreguntaID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.RespuestasReserva.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["PreguntaServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.PreguntasServicio, "PreguntaServicioID", "TextoPregunta", modelo.PreguntaServicioID);
        ViewData["OpcionPreguntaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OpcionesPregunta, "OpcionPreguntaID", "TextoOpcion", modelo.OpcionPreguntaID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("RespuestaReservaID,ReservaID,PreguntaServicioID,OpcionPreguntaID,RespuestaTexto,FechaRespuesta")] RespuestaReserva modelo)
    {
        if (id != modelo.RespuestaReservaID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.RespuestaReservaID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["PreguntaServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.PreguntasServicio, "PreguntaServicioID", "TextoPregunta", modelo.PreguntaServicioID);
        ViewData["OpcionPreguntaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OpcionesPregunta, "OpcionPreguntaID", "TextoOpcion", modelo.OpcionPreguntaID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.RespuestasReserva.Include(r => r.Reserva).Include(r => r.Pregunta).FirstOrDefaultAsync(m => m.RespuestaReservaID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.RespuestasReserva.FindAsync(id);
        if (modelo != null) _context.RespuestasReserva.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.RespuestasReserva.Any(e => e.RespuestaReservaID == id);
}
