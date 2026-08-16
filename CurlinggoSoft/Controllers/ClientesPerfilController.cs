using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class ClientesPerfilController : Controller
{
    private readonly ApplicationDbContext _context;
    public ClientesPerfilController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.ClientesPerfil.Include(c => c.Provincia).Include(c => c.Canton).Include(c => c.Distrito).ToListAsync());

    public async Task<IActionResult> Details(string? id) => id == null ? NotFound() : View(await _context.ClientesPerfil.Include(c => c.Provincia).Include(c => c.Canton).Include(c => c.Distrito).FirstOrDefaultAsync(m => m.ClienteID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["ProvinciaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre");
        ViewData["CantonID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones, "CantonID", "Nombre");
        ViewData["DistritoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Distritos, "DistritoID", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ClienteID,ProvinciaID,CantonID,DistritoID,DireccionExacta,Latitud,Longitud,FechaActualizacion")] ClientePerfil modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ProvinciaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre", modelo.ProvinciaID);
        ViewData["CantonID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones, "CantonID", "Nombre", modelo.CantonID);
        ViewData["DistritoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Distritos, "DistritoID", "Nombre", modelo.DistritoID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.ClientesPerfil.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["ProvinciaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre", modelo.ProvinciaID);
        ViewData["CantonID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones, "CantonID", "Nombre", modelo.CantonID);
        ViewData["DistritoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Distritos, "DistritoID", "Nombre", modelo.DistritoID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? id, [Bind("ClienteID,ProvinciaID,CantonID,DistritoID,DireccionExacta,Latitud,Longitud,FechaActualizacion")] ClientePerfil modelo)
    {
        if (id != modelo.ClienteID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.ClienteID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ProvinciaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre", modelo.ProvinciaID);
        ViewData["CantonID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones, "CantonID", "Nombre", modelo.CantonID);
        ViewData["DistritoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Distritos, "DistritoID", "Nombre", modelo.DistritoID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(string? id) => id == null ? NotFound() : View(await _context.ClientesPerfil.Include(c => c.Provincia).Include(c => c.Canton).Include(c => c.Distrito).FirstOrDefaultAsync(m => m.ClienteID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? id)
    {
        var modelo = await _context.ClientesPerfil.FindAsync(id);
        if (modelo != null) _context.ClientesPerfil.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(string? id) => _context.ClientesPerfil.Any(e => e.ClienteID == id);
}
