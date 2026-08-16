using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class SolicitudesReservaController : Controller
{
    private readonly ApplicationDbContext _context;
    public SolicitudesReservaController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.SolicitudesReserva.Include(r => r.Cliente).Include(r => r.Tecnico).Include(r => r.Servicio).Include(r => r.EstadoReserva).OrderByDescending(r => r.FechaHoraSolicitud).ToListAsync());

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.SolicitudesReserva.Include(r => r.Cliente).Include(r => r.Tecnico).Include(r => r.Servicio).Include(r => r.EstadoReserva).FirstOrDefaultAsync(m => m.ReservaID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["ClienteID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.ClientesPerfil, "ClienteID", "DireccionExacta");
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula");
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio");
        ViewData["EstadoReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosReserva, "EstadoReservaID", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ReservaID,CodigoSeguimiento,ClienteID,TecnicoID,ServicioID,EstadoReservaID,DireccionID,ProvinciaID,CantonID,DistritoID,MontoBaseCotizado,DuracionEstimadaMinutos,FechaHoraProgramada,LatitudServicio,LongitudServicio,FechaHoraSolicitud,FechaHoraCompletada,DireccionServicio,DescripcionProblema,NotasCliente,FechaModificacion")] SolicitudReserva modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClienteID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.ClientesPerfil, "ClienteID", "DireccionExacta", modelo.ClienteID);
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio", modelo.ServicioID);
        ViewData["EstadoReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosReserva, "EstadoReservaID", "Nombre", modelo.EstadoReservaID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.SolicitudesReserva.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["ClienteID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.ClientesPerfil, "ClienteID", "DireccionExacta", modelo.ClienteID);
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio", modelo.ServicioID);
        ViewData["EstadoReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosReserva, "EstadoReservaID", "Nombre", modelo.EstadoReservaID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("ReservaID,CodigoSeguimiento,ClienteID,TecnicoID,ServicioID,EstadoReservaID,DireccionID,ProvinciaID,CantonID,DistritoID,MontoBaseCotizado,DuracionEstimadaMinutos,FechaHoraProgramada,LatitudServicio,LongitudServicio,FechaHoraSolicitud,FechaHoraCompletada,DireccionServicio,DescripcionProblema,NotasCliente,FechaModificacion")] SolicitudReserva modelo)
    {
        if (id != modelo.ReservaID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.ReservaID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClienteID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.ClientesPerfil, "ClienteID", "DireccionExacta", modelo.ClienteID);
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio", modelo.ServicioID);
        ViewData["EstadoReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosReserva, "EstadoReservaID", "Nombre", modelo.EstadoReservaID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.SolicitudesReserva.Include(r => r.Cliente).Include(r => r.Tecnico).Include(r => r.Servicio).Include(r => r.EstadoReserva).FirstOrDefaultAsync(m => m.ReservaID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.SolicitudesReserva.FindAsync(id);
        if (modelo != null) _context.SolicitudesReserva.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.SolicitudesReserva.Any(e => e.ReservaID == id);
}
