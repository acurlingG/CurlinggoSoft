using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;
using System.Security.Claims;

// ADMIN SOLAMENTE: Gestión centralizada de disponibilidad de todos los técnicos
[Authorize(Roles = "Admin")]
public class DisponibilidadTecnicoController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public DisponibilidadTecnicoController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: /DisponibilidadTecnico/Index?tecnicoId=xxx
    // Se agrega filtro por t\u00e9cnico (usando su Usuario asociado para mostrar
    // nombre completo en el combo, en vez de solo la c\u00e9dula).
    public async Task<IActionResult> Index(string? tecnicoId)
    {
        var query = _context.DisponibilidadTecnico.Include(d => d.Tecnico).AsQueryable();

        if (!string.IsNullOrEmpty(tecnicoId))
        {
            query = query.Where(d => d.TecnicoID == tecnicoId);
        }

        ViewBag.TecnicoIDSeleccionado = tecnicoId;
        ViewBag.Tecnicos = await ObtenerListaTecnicosAsync(tecnicoId);

        return View(await query.OrderBy(d => d.TecnicoID).ThenBy(d => d.DiaSemana).ToListAsync());
    }

    // Combo de t\u00e9cnicos mostrando "Nombre Apellidos (email)" en vez de solo
    // el TecnicoID, uniendo TecnicosPerfil con Usuarios por el mismo Id.
    private async Task<Microsoft.AspNetCore.Mvc.Rendering.SelectList> ObtenerListaTecnicosAsync(string? seleccionado)
    {
        var tecnicos = await (
            from t in _context.TecnicosPerfil
            join u in _context.Usuarios on t.TecnicoID equals u.UsuarioID into gu
            from u in gu.DefaultIfEmpty()
            orderby u != null ? u.Nombre : t.IdentificacionCedula
            select new
            {
                t.TecnicoID,
                Texto = u != null ? $"{u.Nombre} {u.Apellidos} ({u.Email})" : t.IdentificacionCedula
            }).ToListAsync();

        return new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tecnicos, "TecnicoID", "Texto", seleccionado);
    }

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
