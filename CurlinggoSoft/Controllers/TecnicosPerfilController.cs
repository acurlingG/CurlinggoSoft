using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class TecnicosPerfilController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    public TecnicosPerfilController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Arma la lista de usuarios con rol "Tecnico" que aun no tienen perfil
    // creado en TecnicosPerfil, para que el admin elija por nombre/correo
    // en vez de escribir a ciegas el ID (GUID) de Identity.
    private async Task<Microsoft.AspNetCore.Mvc.Rendering.SelectList> ObtenerTecnicosDisponiblesAsync(string? tecnicoIdActual = null)
    {
        var usuariosEnRolTecnico = await _userManager.GetUsersInRoleAsync("Tecnico");
        var idsConPerfil = await _context.TecnicosPerfil.Select(t => t.TecnicoID).ToListAsync();
        var disponibles = usuariosEnRolTecnico
            .Where(u => !idsConPerfil.Contains(u.Id) || u.Id == tecnicoIdActual)
            .ToList();

        var usuariosInfo = await _context.Usuarios
            .Where(u => disponibles.Select(d => d.Id).Contains(u.UsuarioID))
            .ToDictionaryAsync(u => u.UsuarioID, u => $"{u.Nombre} {u.Apellidos} ({u.Email})");

        var items = disponibles.Select(u => new { Id = u.Id, Texto = usuariosInfo.TryGetValue(u.Id, out var texto) ? texto : (u.Email ?? u.Id) });
        return new Microsoft.AspNetCore.Mvc.Rendering.SelectList(items, "Id", "Texto", tecnicoIdActual);
    }

    public async Task<IActionResult> Index()
    {
        var tecnicos = await _context.TecnicosPerfil.Include(t => t.ProvinciaCobertura).Include(t => t.CantonCobertura).ToListAsync();
        var usuarios = await _context.Usuarios.ToDictionaryAsync(u => u.UsuarioID, u => u);
        ViewData["Usuarios"] = usuarios;
        return View(tecnicos);
    }

    public async Task<IActionResult> Details(string? id) => id == null ? NotFound() : View(await _context.TecnicosPerfil.Include(t => t.ProvinciaCobertura).Include(t => t.CantonCobertura).FirstOrDefaultAsync(m => m.TecnicoID == id) ?? new());

    public async Task<IActionResult> Create()
    {
        ViewData["TecnicoID"] = await ObtenerTecnicosDisponiblesAsync();
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
            try
            {
                _context.Add(modelo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, ObtenerMensajeErrorGuardado(ex));
            }
        }
        ViewData["TecnicoID"] = await ObtenerTecnicosDisponiblesAsync(modelo.TecnicoID);
        ViewData["ProvinciaCoberturaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Provincias, "ProvinciaID", "Nombre", modelo.ProvinciaCoberturaID);
        ViewData["CantonCoberturaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cantones, "CantonID", "Nombre", modelo.CantonCoberturaID);
        return View(modelo);
    }

    // Traduce errores comunes de restricciones de la base de datos a mensajes entendibles para el usuario
    private static string ObtenerMensajeErrorGuardado(DbUpdateException ex)
    {
        var mensaje = ex.InnerException?.Message ?? ex.Message;
        if (mensaje.Contains("CK_Tecnicos_Calificacion"))
        {
            return "La calificación promedio debe estar entre 0 y 5.";
        }
        return "No se pudo guardar la información. Verifique los datos ingresados e intente nuevamente.";
    }

    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.TecnicosPerfil.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["TecnicoID"] = await ObtenerTecnicosDisponiblesAsync(modelo.TecnicoID);
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
            try
            {
                _context.Update(modelo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.TecnicoID)) return NotFound(); throw; }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, ObtenerMensajeErrorGuardado(ex));
            }
        }
        ViewData["TecnicoID"] = await ObtenerTecnicosDisponiblesAsync(modelo.TecnicoID);
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
