using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class UsuariosController : Controller
{
    private readonly ApplicationDbContext _context;
    public UsuariosController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Usuarios.OrderBy(u => u.Nombre).ToListAsync());

    public async Task<IActionResult> Details(string? id) => id == null ? NotFound() : View(await _context.Usuarios.FirstOrDefaultAsync(m => m.UsuarioID == id) ?? new());

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("UsuarioID,Email,Nombre,Apellidos,Telefono,EstadoUsuario,FechaCreacion,UltimoAcceso")] Usuario modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(modelo);
    }

    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.Usuarios.FindAsync(id);
        if (modelo == null) return NotFound();
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? id, [Bind("UsuarioID,Email,Nombre,Apellidos,Telefono,EstadoUsuario,FechaCreacion,UltimoAcceso")] Usuario modelo)
    {
        if (id != modelo.UsuarioID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.UsuarioID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        return View(modelo);
    }

    public async Task<IActionResult> Delete(string? id) => id == null ? NotFound() : View(await _context.Usuarios.FirstOrDefaultAsync(m => m.UsuarioID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? id)
    {
        var modelo = await _context.Usuarios.FindAsync(id);
        if (modelo != null) _context.Usuarios.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(string? id) => _context.Usuarios.Any(e => e.UsuarioID == id);
}
