using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class LogsIntervencionOperativaController : Controller
{
    private readonly ApplicationDbContext _context;
    public LogsIntervencionOperativaController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.LogsIntervencionOperativa.Include(l => l.Reserva).Include(l => l.UsuarioIntervencion).OrderByDescending(l => l.FechaRegistro).ToListAsync());

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.LogsIntervencionOperativa.Include(l => l.Reserva).Include(l => l.UsuarioIntervencion).FirstOrDefaultAsync(m => m.LogID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio");
        ViewData["UsuarioIntervencionID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("LogID,ReservaID,TipoEvento,DatosEntradaJson,DecisionTomada,ModeloVersion,UsuarioIntervencionID,FechaRegistro")] LogIntervencionOperativa modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["UsuarioIntervencionID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email", modelo.UsuarioIntervencionID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.LogsIntervencionOperativa.Include(l => l.Reserva).Include(l => l.UsuarioIntervencion).FirstOrDefaultAsync(m => m.LogID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.LogsIntervencionOperativa.FindAsync(id);
        if (modelo != null) _context.LogsIntervencionOperativa.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.LogsIntervencionOperativa.Any(e => e.LogID == id);
}
