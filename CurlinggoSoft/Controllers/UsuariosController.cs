using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

// Administra el alta, edicion y baja de usuarios del sistema.
// IMPORTANTE: la autenticacion y los roles viven en ASP.NET Identity
// (AspNetUsers / AspNetRoles / AspNetUserRoles), igual que usa
// AccountController para el login. La tabla de negocio "Usuarios" es
// solo un perfil complementario (nombre, apellidos, telefono, estado)
// enlazado 1:1 por Id con AspNetUsers. Por eso este controlador siempre
// crea/edita/borra en AMBOS lugares a la vez, para que un usuario dado
// de alta aqui pueda iniciar sesion inmediatamente con su rol asignado.
[Authorize(Roles = "Admin")]
public class UsuariosController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsuariosController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index(string? rolFiltro)
    {
        var usuarios = await _context.Usuarios.OrderBy(u => u.Nombre).ToListAsync();
        var roles = new Dictionary<string, string>();
        foreach (var usuario in usuarios)
        {
            var identityUser = await _userManager.FindByIdAsync(usuario.UsuarioID);
            if (identityUser != null)
            {
                var rolesUsuario = await _userManager.GetRolesAsync(identityUser);
                roles[usuario.UsuarioID] = rolesUsuario.FirstOrDefault() ?? "Sin rol";
            }
            else
            {
                roles[usuario.UsuarioID] = "Sin cuenta de acceso";
            }
        }

        if (!string.IsNullOrWhiteSpace(rolFiltro))
        {
            usuarios = usuarios.Where(u => roles.TryGetValue(u.UsuarioID, out var rol) && rol == rolFiltro).ToList();
        }

        ViewData["Roles"] = roles;
        ViewData["RolesDisponibles"] = await _roleManager.Roles.Select(r => r.Name).OrderBy(n => n).ToListAsync();
        ViewData["RolFiltro"] = rolFiltro;
        return View(usuarios);
    }

    public async Task<IActionResult> Details(string? id)
    {
        if (id == null) return NotFound();
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.UsuarioID == id);
        if (usuario == null) return NotFound();

        var identityUser = await _userManager.FindByIdAsync(usuario.UsuarioID);
        ViewData["Rol"] = identityUser != null
            ? (await _userManager.GetRolesAsync(identityUser)).FirstOrDefault() ?? "Sin rol"
            : "Sin cuenta de acceso";

        return View(usuario);
    }

    public IActionResult Create()
    {
        var modelo = new UsuarioAdminVM
        {
            RolesDisponibles = _roleManager.Roles.Select(r => r.Name!).OrderBy(r => r).ToList()
        };
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UsuarioAdminVM modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo.Clave))
        {
            ModelState.AddModelError(nameof(modelo.Clave), "La contraseña es obligatoria al crear un usuario.");
        }

        if (!ModelState.IsValid)
        {
            modelo.RolesDisponibles = _roleManager.Roles.Select(r => r.Name!).OrderBy(r => r).ToList();
            return View(modelo);
        }

        // 1. Crear el usuario en ASP.NET Identity (AspNetUsers).
        var identityUser = new IdentityUser
        {
            UserName = modelo.Email,
            Email = modelo.Email,
            EmailConfirmed = true
        };

        var resultadoCreacion = await _userManager.CreateAsync(identityUser, modelo.Clave!);
        if (!resultadoCreacion.Succeeded)
        {
            foreach (var error in resultadoCreacion.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            modelo.RolesDisponibles = _roleManager.Roles.Select(r => r.Name!).OrderBy(r => r).ToList();
            return View(modelo);
        }

        // 2. Asignar el rol elegido (Admin / Cliente / Tecnico).
        var resultadoRol = await _userManager.AddToRoleAsync(identityUser, modelo.Rol);
        if (!resultadoRol.Succeeded)
        {
            // Revertir el usuario de Identity si no se pudo asignar el rol,
            // para no dejar cuentas huerfanas sin rol valido.
            await _userManager.DeleteAsync(identityUser);
            foreach (var error in resultadoRol.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            modelo.RolesDisponibles = _roleManager.Roles.Select(r => r.Name!).OrderBy(r => r).ToList();
            return View(modelo);
        }

        // 3. Replicar el perfil de negocio usando el MISMO Id de Identity.
        var usuarioNegocio = new Usuario
        {
            UsuarioID = identityUser.Id,
            Email = modelo.Email,
            Nombre = modelo.Nombre,
            Apellidos = modelo.Apellidos,
            Telefono = modelo.Telefono,
            EstadoUsuario = modelo.EstadoUsuario,
            FechaCreacion = DateTime.Now
        };

        _context.Add(usuarioNegocio);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Usuario {modelo.Email} creado con rol {modelo.Rol}.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null) return NotFound();
        var usuarioNegocio = await _context.Usuarios.FindAsync(id);
        if (usuarioNegocio == null) return NotFound();

        var identityUser = await _userManager.FindByIdAsync(id);
        if (identityUser == null) return NotFound();

        var rolActual = (await _userManager.GetRolesAsync(identityUser)).FirstOrDefault();

        var modelo = new UsuarioAdminVM
        {
            UsuarioID = usuarioNegocio.UsuarioID,
            Email = identityUser.Email ?? usuarioNegocio.Email,
            Nombre = usuarioNegocio.Nombre,
            Apellidos = usuarioNegocio.Apellidos,
            Telefono = usuarioNegocio.Telefono,
            EstadoUsuario = usuarioNegocio.EstadoUsuario,
            Rol = rolActual ?? string.Empty,
            RolesDisponibles = _roleManager.Roles.Select(r => r.Name!).OrderBy(r => r).ToList()
        };

        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? id, UsuarioAdminVM modelo)
    {
        if (id == null || id != modelo.UsuarioID) return NotFound();

        // La contraseña es opcional en edicion: solo se valida si se envio.
        ModelState.Remove(nameof(modelo.Clave));

        if (!ModelState.IsValid)
        {
            modelo.RolesDisponibles = _roleManager.Roles.Select(r => r.Name!).OrderBy(r => r).ToList();
            return View(modelo);
        }

        var usuarioNegocio = await _context.Usuarios.FindAsync(id);
        var identityUser = await _userManager.FindByIdAsync(id);
        if (usuarioNegocio == null || identityUser == null) return NotFound();

        // Actualizar datos de negocio.
        usuarioNegocio.Nombre = modelo.Nombre;
        usuarioNegocio.Apellidos = modelo.Apellidos;
        usuarioNegocio.Telefono = modelo.Telefono;
        usuarioNegocio.EstadoUsuario = modelo.EstadoUsuario;
        usuarioNegocio.Email = modelo.Email;

        // Actualizar correo/username en Identity si cambio.
        if (!string.Equals(identityUser.Email, modelo.Email, StringComparison.OrdinalIgnoreCase))
        {
            identityUser.Email = modelo.Email;
            identityUser.UserName = modelo.Email;
            await _userManager.UpdateAsync(identityUser);
        }

        // Cambiar contraseña solo si se proporciono una nueva.
        if (!string.IsNullOrWhiteSpace(modelo.Clave))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
            var resultadoClave = await _userManager.ResetPasswordAsync(identityUser, token, modelo.Clave);
            if (!resultadoClave.Succeeded)
            {
                foreach (var error in resultadoClave.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                modelo.RolesDisponibles = _roleManager.Roles.Select(r => r.Name!).OrderBy(r => r).ToList();
                return View(modelo);
            }
        }

        // Cambiar rol si es distinto al actual (se asume un solo rol por usuario).
        var rolesActuales = await _userManager.GetRolesAsync(identityUser);
        if (!rolesActuales.Contains(modelo.Rol))
        {
            if (rolesActuales.Count > 0)
            {
                await _userManager.RemoveFromRolesAsync(identityUser, rolesActuales);
            }
            await _userManager.AddToRoleAsync(identityUser, modelo.Rol);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!Exists(id)) return NotFound();
            throw;
        }

        TempData["Success"] = $"Usuario {modelo.Email} actualizado.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null) return NotFound();
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.UsuarioID == id);
        if (usuario == null) return NotFound();
        return View(usuario);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? id)
    {
        if (id == null) return NotFound();

        // Borrar primero en Identity (arrastra AspNetUserRoles en cascada)
        // y luego el perfil de negocio.
        var identityUser = await _userManager.FindByIdAsync(id);
        if (identityUser != null)
        {
            await _userManager.DeleteAsync(identityUser);
        }

        var usuarioNegocio = await _context.Usuarios.FindAsync(id);
        if (usuarioNegocio != null)
        {
            _context.Usuarios.Remove(usuarioNegocio);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool Exists(string? id) => _context.Usuarios.Any(e => e.UsuarioID == id);
}

