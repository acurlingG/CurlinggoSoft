using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

namespace CurlinggoSoft.Controllers
{
    // Panel administrativo para revisar y aprobar/rechazar solicitudes de
    // técnico enviadas a través del wizard de 8 pasos. La aprobación invoca
    // el SP transaccional usp_SolicitudTecnico_Aprobar, que crea/actualiza
    // TecnicosPerfil + TecnicoEspecialidades y asigna el rol "Tecnico".
    [Authorize(Roles = "Admin")]
    public class SolicitudTecnicoAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SolicitudTecnicoAdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SolicitudTecnicoAdmin
        // Lista las solicitudes, con filtro opcional por estado.
        public async Task<IActionResult> Index(int? estadoId)
        {
            var query = _context.SolicitudesTecnico
                .Include(s => s.Usuario)
                .Include(s => s.EstadoSolicitud)
                .AsQueryable();

            if (estadoId.HasValue)
            {
                query = query.Where(s => s.EstadoSolicitudTecnicoID == estadoId.Value);
            }
            else
            {
                // Por defecto solo mostramos las que requieren acción del admin
                // (excluye BORRADOR, que el aspirante aún no ha enviado).
                query = query.Where(s => s.EstadoSolicitud!.Codigo != "BORRADOR");
            }

            var solicitudes = await query
                .OrderByDescending(s => s.FechaEnvio ?? s.FechaCreacion)
                .ToListAsync();

            ViewBag.Estados = await _context.EstadosSolicitudTecnico.OrderBy(e => e.Orden).ToListAsync();
            ViewBag.EstadoSeleccionado = estadoId;

            return View(solicitudes);
        }

        // GET: SolicitudTecnicoAdmin/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var solicitud = await _context.SolicitudesTecnico
                .Include(s => s.Usuario)
                .Include(s => s.EstadoSolicitud)
                .Include(s => s.Especialidades).ThenInclude(e => e.Servicio)
                .Include(s => s.Cobertura).ThenInclude(c => c.Provincia)
                .Include(s => s.Cobertura).ThenInclude(c => c.Canton)
                .Include(s => s.Cobertura).ThenInclude(c => c.Distrito)
                .Include(s => s.Documentos).ThenInclude(d => d.TipoDocumento)
                .Include(s => s.BackgroundCheck)
                .FirstOrDefaultAsync(s => s.SolicitudTecnicoID == id);

            if (solicitud == null) return NotFound();

            return View(solicitud);
        }

        // POST: SolicitudTecnicoAdmin/PasarARevision/5
        // Transición manual ENVIADA -> EN_REVISION para indicar que un admin tomó el caso.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasarARevision(long id)
        {
            var solicitud = await _context.SolicitudesTecnico
                .Include(s => s.EstadoSolicitud)
                .FirstOrDefaultAsync(s => s.SolicitudTecnicoID == id);
            if (solicitud == null) return NotFound();

            var estadoEnRevision = await _context.EstadosSolicitudTecnico.FirstOrDefaultAsync(e => e.Codigo == "EN_REVISION");
            if (estadoEnRevision == null)
            {
                TempData["Error"] = "No se encontró el estado 'EN_REVISION' en el catálogo.";
                return RedirectToAction(nameof(Details), new { id });
            }

            solicitud.EstadoSolicitudTecnicoID = estadoEnRevision.EstadoSolicitudTecnicoID;
            solicitud.RevisadoPor = _userManager.GetUserId(User);
            solicitud.FechaRevision = DateTime.Now;
            solicitud.FechaUltimaActualizacion = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["Success"] = "La solicitud pasó a estado 'En Revisión'.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: SolicitudTecnicoAdmin/Aprobar/5
        // Invoca el SP transaccional. Si falla (SqlException), se informa el
        // error sin romper la aplicación (el SP hace ROLLBACK automático).
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aprobar(long id)
        {
            // Obtenemos el ID del usuario administrador autenticado actualmente
            var revisadoPor = _userManager.GetUserId(User) ?? string.Empty;

            try
            {
                // Ejecutamos el Stored Procedure transaccional
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC dbo.usp_SolicitudTecnico_Aprobar @SolicitudTecnicoID = {id}, @RevisadoPor = {revisadoPor}");

                TempData["Success"] = "¡Solicitud aprobada con éxito! El perfil del técnico ha sido creado y el rol de Identity asignado.";
            }
            catch (SqlException ex)
            {
                // Atrapa los errores lanzados por RAISERROR dentro del SP (ej. "La solicitud ya está aprobada", etc.)
                TempData["Error"] = $"No se pudo aprobar la solicitud: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocurrió un error inesperado: {ex.Message}";
            }

            // Redirige de vuelta a la vista de detalles de esa solicitud
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: SolicitudTecnicoAdmin/Rechazar/5
        public async Task<IActionResult> Rechazar(long id)
        {
            var solicitud = await _context.SolicitudesTecnico.FindAsync(id);
            if (solicitud == null) return NotFound();
            return View(solicitud);
        }

        // POST: SolicitudTecnicoAdmin/Rechazar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rechazar(long id, string motivoRechazo)
        {
            var solicitud = await _context.SolicitudesTecnico.FindAsync(id);
            if (solicitud == null) return NotFound();

            if (string.IsNullOrWhiteSpace(motivoRechazo))
            {
                ModelState.AddModelError(nameof(motivoRechazo), "Debe indicar el motivo del rechazo.");
                return View(solicitud);
            }

            var estadoRechazada = await _context.EstadosSolicitudTecnico.FirstOrDefaultAsync(e => e.Codigo == "RECHAZADA");
            if (estadoRechazada == null)
            {
                TempData["Error"] = "No se encontró el estado 'RECHAZADA' en el catálogo.";
                return RedirectToAction(nameof(Details), new { id });
            }

            solicitud.EstadoSolicitudTecnicoID = estadoRechazada.EstadoSolicitudTecnicoID;
            solicitud.MotivoRechazo = motivoRechazo;
            solicitud.RevisadoPor = _userManager.GetUserId(User);
            solicitud.FechaRevision = DateTime.Now;
            solicitud.FechaDecision = DateTime.Now;
            solicitud.FechaUltimaActualizacion = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["Success"] = "La solicitud fue rechazada. Se notificará al aspirante.";
            return RedirectToAction(nameof(Index));
        }
    }
}
