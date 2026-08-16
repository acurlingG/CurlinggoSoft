using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class RespuestasReservaOpcionesController : Controller
{
    private readonly ApplicationDbContext _context;
    public RespuestasReservaOpcionesController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.RespuestasReservaOpciones.Include(r => r.RespuestaReserva).Include(r => r.Opcion).ToListAsync());

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.RespuestasReservaOpciones.Include(r => r.RespuestaReserva).Include(r => r.Opcion).FirstOrDefaultAsync(m => m.RespuestaReservaOpcionID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["RespuestaReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.RespuestasReserva, "RespuestaReservaID", "RespuestaReservaID");
        ViewData["OpcionPreguntaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OpcionesPregunta, "OpcionPreguntaID", "TextoOpcion");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("RespuestaReservaOpcionID,RespuestaReservaID,OpcionPreguntaID")] RespuestaReservaOpcion modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["RespuestaReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.RespuestasReserva, "RespuestaReservaID", "RespuestaReservaID", modelo.RespuestaReservaID);
        ViewData["OpcionPreguntaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OpcionesPregunta, "OpcionPreguntaID", "TextoOpcion", modelo.OpcionPreguntaID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.RespuestasReservaOpciones.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["RespuestaReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.RespuestasReserva, "RespuestaReservaID", "RespuestaReservaID", modelo.RespuestaReservaID);
        ViewData["OpcionPreguntaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OpcionesPregunta, "OpcionPreguntaID", "TextoOpcion", modelo.OpcionPreguntaID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("RespuestaReservaOpcionID,RespuestaReservaID,OpcionPreguntaID")] RespuestaReservaOpcion modelo)
    {
        if (id != modelo.RespuestaReservaOpcionID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.RespuestaReservaOpcionID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["RespuestaReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.RespuestasReserva, "RespuestaReservaID", "RespuestaReservaID", modelo.RespuestaReservaID);
        ViewData["OpcionPreguntaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OpcionesPregunta, "OpcionPreguntaID", "TextoOpcion", modelo.OpcionPreguntaID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.RespuestasReservaOpciones.Include(r => r.RespuestaReserva).Include(r => r.Opcion).FirstOrDefaultAsync(m => m.RespuestaReservaOpcionID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.RespuestasReservaOpciones.FindAsync(id);
        if (modelo != null) _context.RespuestasReservaOpciones.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.RespuestasReservaOpciones.Any(e => e.RespuestaReservaOpcionID == id);
}
