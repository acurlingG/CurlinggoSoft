using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;
using System.Security.Claims;

[Authorize(Roles = "Cliente")]
public class DireccionesClienteController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public DireccionesClienteController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var clienteId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(clienteId))
            return Unauthorized();

        var direcciones = await _context.DireccionesCliente
            .Include(d => d.Cliente)
            .Include(d => d.Provincia)
            .Include(d => d.Canton)
            .Include(d => d.Distrito)
            .Where(d => d.ClienteID == clienteId && d.Activa)
            .ToListAsync();

        return View(direcciones);
    }

    public async Task<IActionResult> Details(long? id)
    {
        if (id == null) return NotFound();

        var clienteId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(clienteId))
            return Unauthorized();

        var direccion = await _context.DireccionesCliente
            .Include(d => d.Cliente)
            .Include(d => d.Provincia)
            .Include(d => d.Canton)
            .Include(d => d.Distrito)
            .FirstOrDefaultAsync(m => m.DireccionID == id && m.ClienteID == clienteId);

        if (direccion == null) return NotFound();
        return View(direccion);
    }

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
        var clienteId = _userManager.GetUserId(User);

        // Validación crítica: solo puede crear direcciones para sí mismo
        if (modelo.ClienteID != clienteId)
            return Unauthorized();

        if (ModelState.IsValid)
        {
            modelo.FechaCreacion = DateTime.Now;
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

        var clienteId = _userManager.GetUserId(User);
        var modelo = await _context.DireccionesCliente.FindAsync(id);
        if (modelo == null || modelo.ClienteID != clienteId) return NotFound();
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

        var clienteId = _userManager.GetUserId(User);
        if (modelo.ClienteID != clienteId) return Unauthorized();

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

    public async Task<IActionResult> Delete(long? id) 
    {
        if (id == null) return NotFound();

        var clienteId = _userManager.GetUserId(User);
        var direccion = await _context.DireccionesCliente
            .Include(d => d.Cliente)
            .Include(d => d.Provincia)
            .Include(d => d.Canton)
            .Include(d => d.Distrito)
            .FirstOrDefaultAsync(m => m.DireccionID == id && m.ClienteID == clienteId);

        if (direccion == null) return NotFound();
        return View(direccion);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var clienteId = _userManager.GetUserId(User);
        var modelo = await _context.DireccionesCliente.FindAsync(id);

        if (modelo == null || modelo.ClienteID != clienteId)
            return Unauthorized();

        _context.DireccionesCliente.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.DireccionesCliente.Any(e => e.DireccionID == id);
}
