using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
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
        // Regla de negocio CURLINGgo: la creación de evaluaciones NO debe hacerse
        // con _context.Add()/SaveChanges(); debe pasar por usp_Evaluacion_Crear
        // para validar el estado de la reserva y las reglas por tipo de evaluación.
        if (ModelState.IsValid)
        {
            await using var connection = new SqlConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync();
            await using var command = new SqlCommand("dbo.usp_Evaluacion_Crear", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add(new SqlParameter("@ReservaID", SqlDbType.BigInt) { Value = modelo.ReservaID });
            command.Parameters.Add(new SqlParameter("@EvaluadorUsuarioID", SqlDbType.NVarChar, 450) { Value = modelo.EvaluadorUsuarioID });
            command.Parameters.Add(new SqlParameter("@EvaluadoUsuarioID", SqlDbType.NVarChar, 450) { Value = (object?)modelo.EvaluadoUsuarioID ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@ServicioID", SqlDbType.Int) { Value = (object?)modelo.ServicioID ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@TipoEvaluacionID", SqlDbType.Int) { Value = modelo.TipoEvaluacionID });
            command.Parameters.Add(new SqlParameter("@Puntuacion", SqlDbType.TinyInt) { Value = modelo.Puntuacion });
            command.Parameters.Add(new SqlParameter("@Comentario", SqlDbType.NVarChar, 1000) { Value = (object?)modelo.Comentario ?? DBNull.Value });

            try
            {
                await command.ExecuteScalarAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
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
