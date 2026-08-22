using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class OfertasTecnicoController : Controller
{
    private readonly ApplicationDbContext _context;
    public OfertasTecnicoController(ApplicationDbContext context) => _context = context;

    // GET: /OfertasTecnico/Index?tecnicoId=xxx
    public async Task<IActionResult> Index(string? tecnicoId)
    {
        var query = _context.OfertasTecnico
            .Include(o => o.Reserva)
            .Include(o => o.Tecnico)
            .Include(o => o.EstadoOferta)
            .AsQueryable();

        if (!string.IsNullOrEmpty(tecnicoId))
        {
            query = query.Where(o => o.TecnicoID == tecnicoId);
        }

        ViewBag.TecnicoIDSeleccionado = tecnicoId;
        ViewBag.Tecnicos = await ObtenerListaTecnicosAsync(tecnicoId);

        return View(await query.OrderByDescending(o => o.FechaEnvio).ToListAsync());
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

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.OfertasTecnico.Include(o => o.Reserva).Include(o => o.Tecnico).Include(o => o.EstadoOferta).FirstOrDefaultAsync(m => m.OfertaTecnicoID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio");
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula");
        ViewData["EstadoOfertaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosOfertaTecnico, "EstadoOfertaID", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("OfertaTecnicoID,ReservaID,TecnicoID,EstadoOfertaID,DistanciaMetros,OrdenOferta,FechaEnvio,FechaExpiracion,FechaRespuesta,Mensaje")] OfertaTecnico modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        ViewData["EstadoOfertaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosOfertaTecnico, "EstadoOfertaID", "Nombre", modelo.EstadoOfertaID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.OfertasTecnico.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        ViewData["EstadoOfertaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosOfertaTecnico, "EstadoOfertaID", "Nombre", modelo.EstadoOfertaID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("OfertaTecnicoID,ReservaID,TecnicoID,EstadoOfertaID,DistanciaMetros,OrdenOferta,FechaEnvio,FechaExpiracion,FechaRespuesta,Mensaje")] OfertaTecnico modelo)
    {
        if (id != modelo.OfertaTecnicoID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.OfertaTecnicoID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        ViewData["EstadoOfertaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosOfertaTecnico, "EstadoOfertaID", "Nombre", modelo.EstadoOfertaID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.OfertasTecnico.Include(o => o.Reserva).Include(o => o.Tecnico).Include(o => o.EstadoOferta).FirstOrDefaultAsync(m => m.OfertaTecnicoID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.OfertasTecnico.FindAsync(id);
        if (modelo != null) _context.OfertasTecnico.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.OfertasTecnico.Any(e => e.OfertaTecnicoID == id);
}
