using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class AuditoriaController : Controller
{
    private readonly ApplicationDbContext _context;
    public AuditoriaController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Auditoria.Include(a => a.Usuario).OrderByDescending(a => a.FechaEvento).ToListAsync());

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.Auditoria.Include(a => a.Usuario).FirstOrDefaultAsync(m => m.AuditoriaID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["UsuarioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AuditoriaID,UsuarioID,TablaAfectada,RegistroID,Operacion,ValoresAnterioresJson,ValoresNuevosJson,FechaEvento,DireccionIP,CorrelationID")] Auditoria modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["UsuarioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email", modelo.UsuarioID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.Auditoria.Include(a => a.Usuario).FirstOrDefaultAsync(m => m.AuditoriaID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.Auditoria.FindAsync(id);
        if (modelo != null) _context.Auditoria.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.Auditoria.Any(e => e.AuditoriaID == id);
}
