using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class TecnicosPerfilController : Controller
{
    private readonly ApplicationDbContext _context;
    public TecnicosPerfilController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.TecnicosPerfil.Include(t => t.ProvinciaCobertura).Include(t => t.CantonCobertura).ToListAsync());

    public async Task<IActionResult> Details(string? id) => id == null ? NotFound() : View(await _context.TecnicosPerfil.Include(t => t.ProvinciaCobertura).Include(t => t.CantonCobertura).FirstOrDefaultAsync(m => m.TecnicoID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["ProvinciaCoberturaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre");
        ViewData["CantonCoberturaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones, "CantonID", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TecnicoID,IdentificacionCedula,EstadoVerificacion,CalificacionPromedio,Disponible,ProvinciaCoberturaID,CantonCoberturaID,LatitudActual,LongitudActual,FechaVerificacion")] TecnicoPerfil modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ProvinciaCoberturaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre", modelo.ProvinciaCoberturaID);
        ViewData["CantonCoberturaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones, "CantonID", "Nombre", modelo.CantonCoberturaID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.TecnicosPerfil.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["ProvinciaCoberturaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre", modelo.ProvinciaCoberturaID);
        ViewData["CantonCoberturaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones, "CantonID", "Nombre", modelo.CantonCoberturaID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? id, [Bind("TecnicoID,IdentificacionCedula,EstadoVerificacion,CalificacionPromedio,Disponible,ProvinciaCoberturaID,CantonCoberturaID,LatitudActual,LongitudActual,FechaVerificacion")] TecnicoPerfil modelo)
    {
        if (id != modelo.TecnicoID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.TecnicoID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ProvinciaCoberturaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre", modelo.ProvinciaCoberturaID);
        ViewData["CantonCoberturaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones, "CantonID", "Nombre", modelo.CantonCoberturaID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(string? id) => id == null ? NotFound() : View(await _context.TecnicosPerfil.Include(t => t.ProvinciaCobertura).Include(t => t.CantonCobertura).FirstOrDefaultAsync(m => m.TecnicoID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? id)
    {
        var modelo = await _context.TecnicosPerfil.FindAsync(id);
        if (modelo != null) _context.TecnicosPerfil.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(string? id) => _context.TecnicosPerfil.Any(e => e.TecnicoID == id);
}
