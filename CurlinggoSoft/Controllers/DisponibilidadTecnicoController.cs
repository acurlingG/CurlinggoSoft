using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class DisponibilidadTecnicoController : Controller
{
    private readonly ApplicationDbContext _context;
    public DisponibilidadTecnicoController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.DisponibilidadTecnico.Include(d => d.Tecnico).ToListAsync());

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.DisponibilidadTecnico.Include(d => d.Tecnico).FirstOrDefaultAsync(m => m.DisponibilidadID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DisponibilidadID,TecnicoID,DiaSemana,HoraInicio,HoraFin,Activa")] DisponibilidadTecnico modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.DisponibilidadTecnico.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("DisponibilidadID,TecnicoID,DiaSemana,HoraInicio,HoraFin,Activa")] DisponibilidadTecnico modelo)
    {
        if (id != modelo.DisponibilidadID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.DisponibilidadID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.DisponibilidadTecnico.Include(d => d.Tecnico).FirstOrDefaultAsync(m => m.DisponibilidadID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.DisponibilidadTecnico.FindAsync(id);
        if (modelo != null) _context.DisponibilidadTecnico.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.DisponibilidadTecnico.Any(e => e.DisponibilidadID == id);
}
