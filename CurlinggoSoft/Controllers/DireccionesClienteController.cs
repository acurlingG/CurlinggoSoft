using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class DireccionesClienteController : Controller
{
    private readonly ApplicationDbContext _context;
    public DireccionesClienteController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.DireccionesCliente.Include(d => d.Cliente).Include(d => d.Provincia).Include(d => d.Canton).Include(d => d.Distrito).ToListAsync());

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.DireccionesCliente.Include(d => d.Cliente).Include(d => d.Provincia).Include(d => d.Canton).Include(d => d.Distrito).FirstOrDefaultAsync(m => m.DireccionID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["ClienteID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.ClientesPerfil, "ClienteID", "DireccionExacta");
        ViewData["ProvinciaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre");
        ViewData["CantonID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones, "CantonID", "Nombre");
        ViewData["DistritoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Distritos, "DistritoID", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DireccionID,ClienteID,NombreDireccion,ProvinciaID,CantonID,DistritoID,DireccionExacta,Latitud,Longitud,EsPrincipal,Activa,FechaCreacion")] DireccionCliente modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClienteID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.ClientesPerfil, "ClienteID", "DireccionExacta", modelo.ClienteID);
        ViewData["ProvinciaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre", modelo.ProvinciaID);
        ViewData["CantonID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones, "CantonID", "Nombre", modelo.CantonID);
        ViewData["DistritoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Distritos, "DistritoID", "Nombre", modelo.DistritoID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.DireccionesCliente.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["ClienteID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.ClientesPerfil, "ClienteID", "DireccionExacta", modelo.ClienteID);
        ViewData["ProvinciaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre", modelo.ProvinciaID);
        ViewData["CantonID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones, "CantonID", "Nombre", modelo.CantonID);
        ViewData["DistritoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Distritos, "DistritoID", "Nombre", modelo.DistritoID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("DireccionID,ClienteID,NombreDireccion,ProvinciaID,CantonID,DistritoID,DireccionExacta,Latitud,Longitud,EsPrincipal,Activa,FechaCreacion")] DireccionCliente modelo)
    {
        if (id != modelo.DireccionID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.DireccionID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClienteID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.ClientesPerfil, "ClienteID", "DireccionExacta", modelo.ClienteID);
        ViewData["ProvinciaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre", modelo.ProvinciaID);
        ViewData["CantonID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones, "CantonID", "Nombre", modelo.CantonID);
        ViewData["DistritoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Distritos, "DistritoID", "Nombre", modelo.DistritoID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.DireccionesCliente.Include(d => d.Cliente).Include(d => d.Provincia).Include(d => d.Canton).Include(d => d.Distrito).FirstOrDefaultAsync(m => m.DireccionID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.DireccionesCliente.FindAsync(id);
        if (modelo != null) _context.DireccionesCliente.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.DireccionesCliente.Any(e => e.DireccionID == id);
}
