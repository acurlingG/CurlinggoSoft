using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class OpcionesPreguntaController : Controller
{
    private readonly ApplicationDbContext _context;
    public OpcionesPreguntaController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.OpcionesPregunta.Include(o => o.Pregunta).OrderBy(o => o.Orden).ToListAsync());

    public async Task<IActionResult> Details(int? id) => id == null ? NotFound() : View(await _context.OpcionesPregunta.Include(o => o.Pregunta).FirstOrDefaultAsync(m => m.OpcionPreguntaID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["PreguntaServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.PreguntasServicio, "PreguntaServicioID", "TextoPregunta");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("OpcionPreguntaID,PreguntaServicioID,TextoOpcion,Valor,Orden,Activa")] OpcionPregunta modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["PreguntaServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.PreguntasServicio, "PreguntaServicioID", "TextoPregunta", modelo.PreguntaServicioID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.OpcionesPregunta.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["PreguntaServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.PreguntasServicio, "PreguntaServicioID", "TextoPregunta", modelo.PreguntaServicioID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("OpcionPreguntaID,PreguntaServicioID,TextoOpcion,Valor,Orden,Activa")] OpcionPregunta modelo)
    {
        if (id != modelo.OpcionPreguntaID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.OpcionPreguntaID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["PreguntaServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.PreguntasServicio, "PreguntaServicioID", "TextoPregunta", modelo.PreguntaServicioID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(int? id) => id == null ? NotFound() : View(await _context.OpcionesPregunta.Include(o => o.Pregunta).FirstOrDefaultAsync(m => m.OpcionPreguntaID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var modelo = await _context.OpcionesPregunta.FindAsync(id);
        if (modelo != null) _context.OpcionesPregunta.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(int? id) => _context.OpcionesPregunta.Any(e => e.OpcionPreguntaID == id);
}
