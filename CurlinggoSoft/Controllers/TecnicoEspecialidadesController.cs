using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class TecnicoEspecialidadesController : Controller
{
    private readonly ApplicationDbContext _context;
    public TecnicoEspecialidadesController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.TecnicoEspecialidades.Include(t => t.Tecnico).Include(t => t.Servicio).ToListAsync());

    public async Task<IActionResult> Details(string? tecnicoId, int? servicioId) => (tecnicoId == null || servicioId == null) ? NotFound() : View(await _context.TecnicoEspecialidades.Include(t => t.Tecnico).Include(t => t.Servicio).FirstOrDefaultAsync(m => m.TecnicoID == tecnicoId && m.ServicioID == servicioId) ?? new());

    public IActionResult Create()
    {
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula");
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TecnicoID,ServicioID,AniosExperiencia")] TecnicoEspecialidad modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio", modelo.ServicioID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(string? tecnicoId, int? servicioId) => (tecnicoId == null || servicioId == null) ? NotFound() : View(await _context.TecnicoEspecialidades.Include(t => t.Tecnico).Include(t => t.Servicio).FirstOrDefaultAsync(m => m.TecnicoID == tecnicoId && m.ServicioID == servicioId) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string tecnicoId, int servicioId)
    {
        var modelo = await _context.TecnicoEspecialidades.FindAsync(tecnicoId, servicioId);
        if (modelo != null) _context.TecnicoEspecialidades.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
