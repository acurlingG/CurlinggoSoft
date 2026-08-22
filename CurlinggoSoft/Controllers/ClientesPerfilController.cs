using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class ClientesPerfilController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    public ClientesPerfilController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Arma la lista de usuarios con rol "Cliente" que aun no tienen perfil
    // creado en ClientesPerfil, para que el admin elija por nombre/correo
    // en vez de escribir a ciegas el ID (GUID) de Identity.
    private async Task<Microsoft.AspNetCore.Mvc.Rendering.SelectList> ObtenerClientesDisponiblesAsync(string? clienteIdActual = null)
    {
        var usuariosEnRolCliente = await _userManager.GetUsersInRoleAsync("Cliente");
        var idsConPerfil = await _context.ClientesPerfil.Select(c => c.ClienteID).ToListAsync();
        var disponibles = usuariosEnRolCliente
            .Where(u => !idsConPerfil.Contains(u.Id) || u.Id == clienteIdActual)
            .ToList();

        var usuariosInfo = await _context.Usuarios
            .Where(u => disponibles.Select(d => d.Id).Contains(u.UsuarioID))
            .ToDictionaryAsync(u => u.UsuarioID, u => $"{u.Nombre} {u.Apellidos} ({u.Email})");

        var items = disponibles.Select(u => new { Id = u.Id, Texto = usuariosInfo.TryGetValue(u.Id, out var texto) ? texto : (u.Email ?? u.Id) });
        return new Microsoft.AspNetCore.Mvc.Rendering.SelectList(items, "Id", "Texto", clienteIdActual);
    }

    public async Task<IActionResult> Index()
    {
        var clientes = await _context.ClientesPerfil.Include(c => c.Provincia).Include(c => c.Canton).Include(c => c.Distrito).ToListAsync();
        var usuarios = await _context.Usuarios.ToDictionaryAsync(u => u.UsuarioID, u => u);
        ViewData["Usuarios"] = usuarios;
        return View(clientes);
    }

    public async Task<IActionResult> Details(string? id) => id == null ? NotFound() : View(await _context.ClientesPerfil.Include(c => c.Provincia).Include(c => c.Canton).Include(c => c.Distrito).FirstOrDefaultAsync(m => m.ClienteID == id) ?? new());

    public async Task<IActionResult> Create()
    {
        ViewData["ClienteID"] = await ObtenerClientesDisponiblesAsync();
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
        ViewData["ClienteID"] = await ObtenerClientesDisponiblesAsync(modelo.ClienteID);
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
        ViewData["ClienteID"] = await ObtenerClientesDisponiblesAsync(modelo.ClienteID);
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
        ViewData["ClienteID"] = await ObtenerClientesDisponiblesAsync(modelo.ClienteID);
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
