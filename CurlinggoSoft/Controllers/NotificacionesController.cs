using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class NotificacionesController : Controller
{
    private readonly ApplicationDbContext _context;
    public NotificacionesController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Notificaciones.Include(n => n.Usuario).Include(n => n.Reserva).OrderByDescending(n => n.FechaCreacion).ToListAsync());

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.Notificaciones.Include(n => n.Usuario).Include(n => n.Reserva).Include(n => n.OfertaTecnico).FirstOrDefaultAsync(m => m.NotificacionID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["UsuarioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email");
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio");
        ViewData["OfertaTecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OfertasTecnico, "OfertaTecnicoID", "OfertaTecnicoID");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NotificacionID,UsuarioID,ReservaID,OfertaTecnicoID,TipoNotificacion,Titulo,Mensaje,Leida,FechaCreacion,FechaLectura")] Notificacion modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["UsuarioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email", modelo.UsuarioID);
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["OfertaTecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OfertasTecnico, "OfertaTecnicoID", "OfertaTecnicoID", modelo.OfertaTecnicoID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.Notificaciones.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["UsuarioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email", modelo.UsuarioID);
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["OfertaTecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OfertasTecnico, "OfertaTecnicoID", "OfertaTecnicoID", modelo.OfertaTecnicoID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("NotificacionID,UsuarioID,ReservaID,OfertaTecnicoID,TipoNotificacion,Titulo,Mensaje,Leida,FechaCreacion,FechaLectura")] Notificacion modelo)
    {
        if (id != modelo.NotificacionID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.NotificacionID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["UsuarioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email", modelo.UsuarioID);
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["OfertaTecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.OfertasTecnico, "OfertaTecnicoID", "OfertaTecnicoID", modelo.OfertaTecnicoID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.Notificaciones.Include(n => n.Usuario).Include(n => n.Reserva).FirstOrDefaultAsync(m => m.NotificacionID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.Notificaciones.FindAsync(id);
        if (modelo != null) _context.Notificaciones.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.Notificaciones.Any(e => e.NotificacionID == id);
}
