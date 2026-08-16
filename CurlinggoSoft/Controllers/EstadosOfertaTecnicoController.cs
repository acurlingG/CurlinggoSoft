using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class EstadosOfertaTecnicoController : Controller
{
    private readonly ApplicationDbContext _context;
    public EstadosOfertaTecnicoController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.EstadosOfertaTecnico.ToListAsync());

    public async Task<IActionResult> Details(int? id) => id == null ? NotFound() : View(await _context.EstadosOfertaTecnico.FirstOrDefaultAsync(m => m.EstadoOfertaID == id) ?? new());

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EstadoOfertaID,Codigo,Nombre")] EstadoOfertaTecnico modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(modelo);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.EstadosOfertaTecnico.FindAsync(id);
        if (modelo == null) return NotFound();
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("EstadoOfertaID,Codigo,Nombre")] EstadoOfertaTecnico modelo)
    {
        if (id != modelo.EstadoOfertaID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.EstadoOfertaID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        return View(modelo);
    }

    public async Task<IActionResult> Delete(int? id) => id == null ? NotFound() : View(await _context.EstadosOfertaTecnico.FirstOrDefaultAsync(m => m.EstadoOfertaID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var modelo = await _context.EstadosOfertaTecnico.FindAsync(id);
        if (modelo != null) _context.EstadosOfertaTecnico.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(int? id) => _context.EstadosOfertaTecnico.Any(e => e.EstadoOfertaID == id);
}
