using CurlinggoSoft.Hubs;
using CurlinggoSoft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CurlinggoSoft.Controllers
{
    // Panel del técnico: trabajos asignados, radar de ofertas y actualización
    // de su ubicación actual.
    //
    // NOTA: ajusta "Tecnico" al nombre exacto de tu rol en AspNetRoles si es
    // distinto.
    [Authorize(Roles = "Tecnico")]
    public class TecnicoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificacionesHub> _hub;

        public TecnicoController(ApplicationDbContext context, IHubContext<NotificacionesHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        private string TecnicoId =>
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        // GET: /Tecnico/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var trabajos = await _context.SolicitudesReserva
                .Include(r => r.Servicio)
                .Include(r => r.EstadoReserva)
                .Where(r => r.TecnicoID == TecnicoId)
                .OrderByDescending(r => r.FechaHoraProgramada)
                .ToListAsync();

            return View(trabajos);
        }

        // GET: /Tecnico/OfertasDisponibles
        [HttpGet]
        public async Task<IActionResult> OfertasDisponibles()
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC dbo.usp_OfertaTecnico_ExpirarVencidas");

            var ofertas = await _context.OfertasTecnico
                .Include(o => o.Reserva).ThenInclude(r => r!.Servicio)
                .Include(o => o.EstadoOferta)
                .Where(o => o.TecnicoID == TecnicoId &&
                            o.EstadoOferta!.Codigo == "PENDIENTE" &&
                            (o.FechaExpiracion == null || o.FechaExpiracion > DateTime.Now))
                .OrderBy(o => o.FechaExpiracion)
                .ToListAsync();

            return View(ofertas);
        }

        // POST: /Tecnico/AceptarOferta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AceptarOferta(long ofertaId)
        {
            // Capturamos la ReservaID ANTES de aceptar, para poder avisarle
            // después a los demás técnicos que también la tenían ofertada.
            var reservaId = await _context.OfertasTecnico
                .Where(o => o.OfertaTecnicoID == ofertaId)
                .Select(o => o.ReservaID)
                .FirstOrDefaultAsync();

            try
            {
                var pOferta = new SqlParameter("@OfertaTecnicoID", ofertaId);
                var pTecnico = new SqlParameter("@TecnicoID", TecnicoId);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_OfertaTecnico_Aceptar @OfertaTecnicoID, @TecnicoID",
                    pOferta, pTecnico);

                TempData["Success"] = "¡Servicio asignado! Ya aparece en tus trabajos.";

                // El SP ya marcó como RECHAZADA cualquier otra oferta pendiente
                // de esta misma reserva. Avisamos por SignalR a esos técnicos
                // para que su pantalla se refresque al instante y ya no vean
                // ni puedan intentar aceptar una oferta que dejó de existir.
                if (reservaId > 0)
                {
                    var otrosTecnicos = await _context.OfertasTecnico
                        .Where(o => o.ReservaID == reservaId && o.TecnicoID != TecnicoId)
                        .Select(o => o.TecnicoID)
                        .Distinct()
                        .ToListAsync();

                    foreach (var otroTecnicoId in otrosTecnicos)
                    {
                        await _hub.Clients.Group(NotificacionesHub.GrupoTecnico(otroTecnicoId))
                            .SendAsync("OfertaYaTomada", new { reservaId });
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var mensaje = ex.InnerException?.Message ?? ex.Message;

                TempData["Error"] = mensaje switch
                {
                    var m when m.Contains("no existe, ya fue respondida") =>
                        "Esa solicitud ya no está disponible — probablemente otro técnico la aceptó primero.",
                    var m when m.Contains("ya expiro") =>
                        "Esa oferta ya expiró. Se está reasignando a otro técnico.",
                    var m when m.Contains("ya fue asignada a otro tecnico") =>
                        "El cliente ya quedó asignado a otro técnico.",
                    _ => "No se pudo aceptar la solicitud: " + mensaje
                };

                return RedirectToAction(nameof(OfertasDisponibles));
            }
        }

        // POST: /Tecnico/ActualizarUbicacion
        [HttpPost]
        public async Task<IActionResult> ActualizarUbicacion([FromForm] decimal latitud, [FromForm] decimal longitud)
        {
            if (string.IsNullOrEmpty(TecnicoId))
                return Unauthorized();

            try
            {
                var pTecnico = new SqlParameter("@TecnicoID", TecnicoId);
                var pLat = new SqlParameter("@Latitud", latitud);
                var pLon = new SqlParameter("@Longitud", longitud);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_Tecnico_ActualizarUbicacion @TecnicoID, @Latitud, @Longitud",
                    pTecnico, pLat, pLon);

                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: /Tecnico/ContarOfertasPendientes
        // Se mantiene como respaldo del badge cuando SignalR aún no ha
        // conectado o se cayó momentáneamente.
        [HttpGet]
        public async Task<IActionResult> ContarOfertasPendientes()
        {
            var count = await _context.OfertasTecnico
                .Include(o => o.EstadoOferta)
                .Where(o => o.TecnicoID == TecnicoId &&
                            o.EstadoOferta!.Codigo == "PENDIENTE" &&
                            (o.FechaExpiracion == null || o.FechaExpiracion > DateTime.Now))
                .CountAsync();

            return Json(new { count });
        }

        // GET: /Tecnico/GetProvincias
        // Alimenta el <select> de "Mi zona de cobertura" (Nivel 3 de respaldo,
        // para el técnico que nunca da permiso de GPS).
        [HttpGet]
        public async Task<IActionResult> GetProvincias()
        {
            var provincias = await _context.Provincias
                .Where(p => p.Activa)
                .OrderBy(p => p.Nombre)
                .Select(p => new { id = p.ProvinciaID, nombre = p.Nombre })
                .ToListAsync();

            return Json(provincias);
        }

        // GET: /Tecnico/GetCantones?provinciaId=1
        [HttpGet]
        public async Task<IActionResult> GetCantones(int provinciaId)
        {
            var cantones = await _context.Cantones
                .Where(c => c.ProvinciaID == provinciaId && c.Activo)
                .OrderBy(c => c.Nombre)
                .Select(c => new { id = c.CantonID, nombre = c.Nombre })
                .ToListAsync();

            return Json(cantones);
        }

        // POST: /Tecnico/ActualizarZonaCobertura
        // Guarda ProvinciaCoberturaID/CantonCoberturaID en TecnicosPerfil.
        // Esto es lo que le da al técnico el Nivel 3 del match: aunque nunca
        // otorgue permiso de GPS, con esto configurado una sola vez sigue
        // apareciendo en usp_Reserva_BuscarTecnicosDisponibles por zona.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarZonaCobertura(int provinciaCoberturaId, int cantonCoberturaId)
        {
            if (string.IsNullOrEmpty(TecnicoId))
                return Unauthorized();

            try
            {
                var pTecnico = new SqlParameter("@TecnicoID", TecnicoId);
                var pProvincia = new SqlParameter("@ProvinciaCoberturaID", provinciaCoberturaId);
                var pCanton = new SqlParameter("@CantonCoberturaID", cantonCoberturaId);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_Tecnico_ActualizarZonaCobertura @TecnicoID, @ProvinciaCoberturaID, @CantonCoberturaID",
                    pTecnico, pProvincia, pCanton);

                return Ok();
            }
            catch (Exception ex)
            {
                // Casi siempre THROW 52004 del SP: el cantón no pertenece a
                // la provincia seleccionada (puede pasar si el <select> del
                // cliente quedó desincronizado por caché del navegador).
                var mensaje = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { error = mensaje });
            }
        }
    }
}