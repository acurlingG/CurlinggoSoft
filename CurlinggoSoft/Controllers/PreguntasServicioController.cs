using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class PreguntasServicioController : Controller
{
    private readonly ApplicationDbContext _context;
    public PreguntasServicioController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.PreguntasServicio.Include(p => p.Servicio).OrderBy(p => p.Orden).ToListAsync());

    public async Task<IActionResult> Details(int? id) => id == null ? NotFound() : View(await _context.PreguntasServicio.Include(p => p.Servicio).FirstOrDefaultAsync(m => m.PreguntaServicioID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PreguntaServicioID,ServicioID,TextoPregunta,TipoRespuesta,Obligatoria,Orden,Activa")] PreguntaServicio modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio", modelo.ServicioID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.PreguntasServicio.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio", modelo.ServicioID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("PreguntaServicioID,ServicioID,TextoPregunta,TipoRespuesta,Obligatoria,Orden,Activa")] PreguntaServicio modelo)
    {
        if (id != modelo.PreguntaServicioID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.PreguntaServicioID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio", modelo.ServicioID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(int? id) => id == null ? NotFound() : View(await _context.PreguntasServicio.Include(p => p.Servicio).FirstOrDefaultAsync(m => m.PreguntaServicioID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var modelo = await _context.PreguntasServicio.FindAsync(id);
        if (modelo != null) _context.PreguntasServicio.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(int? id) => _context.PreguntasServicio.Any(e => e.PreguntaServicioID == id);
}
