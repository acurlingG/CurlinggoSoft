using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class EvaluacionesController : Controller
{
    private readonly ApplicationDbContext _context;
    public EvaluacionesController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Evaluaciones.Include(e => e.Reserva).Include(e => e.Evaluador).Include(e => e.Evaluado).Include(e => e.TipoEvaluacion).OrderByDescending(e => e.FechaEvaluacion).ToListAsync());

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.Evaluaciones.Include(e => e.Reserva).Include(e => e.Evaluador).Include(e => e.Evaluado).Include(e => e.Servicio).Include(e => e.TipoEvaluacion).FirstOrDefaultAsync(m => m.EvaluacionID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio");
        ViewData["EvaluadorUsuarioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email");
        ViewData["EvaluadoUsuarioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email");
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio");
        ViewData["TipoEvaluacionID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TiposEvaluacion, "TipoEvaluacionID", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EvaluacionID,ReservaID,EvaluadorUsuarioID,EvaluadoUsuarioID,ServicioID,TipoEvaluacionID,Puntuacion,Comentario,FechaEvaluacion,Activa")] Evaluacion modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["EvaluadorUsuarioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email", modelo.EvaluadorUsuarioID);
        ViewData["EvaluadoUsuarioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email", modelo.EvaluadoUsuarioID);
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio", modelo.ServicioID);
        ViewData["TipoEvaluacionID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TiposEvaluacion, "TipoEvaluacionID", "Nombre", modelo.TipoEvaluacionID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.Evaluaciones.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["EvaluadorUsuarioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email", modelo.EvaluadorUsuarioID);
        ViewData["EvaluadoUsuarioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email", modelo.EvaluadoUsuarioID);
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio", modelo.ServicioID);
        ViewData["TipoEvaluacionID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TiposEvaluacion, "TipoEvaluacionID", "Nombre", modelo.TipoEvaluacionID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("EvaluacionID,ReservaID,EvaluadorUsuarioID,EvaluadoUsuarioID,ServicioID,TipoEvaluacionID,Puntuacion,Comentario,FechaEvaluacion,Activa")] Evaluacion modelo)
    {
        if (id != modelo.EvaluacionID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.EvaluacionID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.SolicitudesReserva, "ReservaID", "DireccionServicio", modelo.ReservaID);
        ViewData["EvaluadorUsuarioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email", modelo.EvaluadorUsuarioID);
        ViewData["EvaluadoUsuarioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Usuarios, "UsuarioID", "Email", modelo.EvaluadoUsuarioID);
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio", modelo.ServicioID);
        ViewData["TipoEvaluacionID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TiposEvaluacion, "TipoEvaluacionID", "Nombre", modelo.TipoEvaluacionID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.Evaluaciones.Include(e => e.Reserva).Include(e => e.Evaluador).Include(e => e.TipoEvaluacion).FirstOrDefaultAsync(m => m.EvaluacionID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.Evaluaciones.FindAsync(id);
        if (modelo != null) _context.Evaluaciones.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.Evaluaciones.Any(e => e.EvaluacionID == id);
}
