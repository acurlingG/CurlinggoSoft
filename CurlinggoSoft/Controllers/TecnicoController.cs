using CurlinggoSoft.Hubs;
using CurlinggoSoft.Models;
using CurlinggoSoft.Services;
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
    public partial class TecnicoController : Controller
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
                .OrderByDescending(r => r.ReservaID)
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

                // Datos del técnico para mostrarle al cliente en Paso6. Se
                // consultan aparte (no via reserva.Tecnico) para no depender
                // de qué nombre de propiedad de navegación exista en el
                // modelo — Usuarios + TecnicosPerfil es la fuente de verdad.
                var datosTecnico = await _context.Usuarios
                    .Where(u => u.UsuarioID == TecnicoId)
                    .Select(u => new
                    {
                        nombre = u.Nombre,
                        apellidos = u.Apellidos,
                        telefono = u.Telefono
                    })
                    .FirstOrDefaultAsync();

                var calificacion = await _context.TecnicosPerfil
                    .Where(t => t.TecnicoID == TecnicoId)
                    .Select(t => t.CalificacionPromedio)
                    .FirstOrDefaultAsync();

                // PUSH al cliente: reemplaza el "Buscando Técnico..." estático
                // de Paso6ConfirmacionExitosa.cshtml por los datos reales.
                if (reservaId > 0)
                {
                    await _hub.Clients.Group(NotificacionesHub.GrupoReserva(reservaId))
                        .SendAsync("TecnicoAsignado", new
                        {
                            nombre = datosTecnico?.nombre ?? "Técnico",
                            apellidos = datosTecnico?.apellidos ?? "",
                            telefono = datosTecnico?.telefono ?? "",
                            calificacion = calificacion
                        });
                }

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

        // POST: /Tecnico/AvanzarEstado
        // Avanza la reserva por la máquina de estados ya validada en
        // usp_Reserva_CambiarEstado (ASIGNADA→EN_CAMINO→EN_PROCESO→COMPLETADA,
        // o CANCELADA desde cualquiera de esos). El SP rechaza cualquier
        // transición fuera de esa secuencia con THROW 50008, así que aquí
        // solo hace falta traducir el error, no revalidar la lógica de
        // negocio dos veces.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AvanzarEstado(long reservaId, string nuevoEstadoCodigo)
        {
            try
            {
                var esDelTecnico = await _context.SolicitudesReserva
                    .AnyAsync(r => r.ReservaID == reservaId && r.TecnicoID == TecnicoId);

                if (!esDelTecnico)
                {
                    TempData["Error"] = "Esa reserva no está asignada a tu cuenta.";
                    return RedirectToAction(nameof(Index));
                }

                var estadoNuevoId = await _context.EstadosReserva
                    .Where(e => e.Codigo == nuevoEstadoCodigo)
                    .Select(e => e.EstadoReservaID)
                    .FirstOrDefaultAsync();

                if (estadoNuevoId == 0)
                {
                    TempData["Error"] = "Estado no reconocido.";
                    return RedirectToAction(nameof(Index));
                }

                var pReserva = new SqlParameter("@ReservaID", reservaId);
                var pEstado = new SqlParameter("@EstadoNuevoID", estadoNuevoId);
                var pUsuario = new SqlParameter("@UsuarioModificadorID", TecnicoId);
                var pObs = new SqlParameter("@Observaciones",
                    (object?)$"Actualizado por el técnico a {nuevoEstadoCodigo}." ?? DBNull.Value);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_Reserva_CambiarEstado @ReservaID, @EstadoNuevoID, @UsuarioModificadorID, @Observaciones",
                    pReserva, pEstado, pUsuario, pObs);

                // PUSH al cliente: actualiza el tracking en Paso6 en tiempo
                // real, sin que tenga que recargar la página.
                await _hub.Clients.Group(NotificacionesHub.GrupoReserva(reservaId))
                    .SendAsync("EstadoActualizado", new { estado = nuevoEstadoCodigo });

                TempData["Success"] = "Estado actualizado.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var mensaje = ex.InnerException?.Message ?? ex.Message;
                TempData["Error"] = mensaje.Contains("Transicion de estado no permitida")
                    ? "Ese cambio de estado no es válido desde el estado actual de la reserva."
                    : "No se pudo actualizar el estado: " + mensaje;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Tecnico/CancelarReserva
        // Permite al tecnico cancelar una reserva que tiene asignada,
        // mientras el estado siga siendo SOLICITADA, ASIGNADA o EN_CAMINO.
        // Misma ventana de gracia que para el cliente (ver
        // ReservaCancelacionHelper): sin penalizacion si pasaron 10 min o
        // menos desde ASIGNADA, O el propio tecnico todavia no marco
        // EN_CAMINO. Solo hay penalizacion simulada si ambas fallan.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarReserva(long reservaId, string motivoCodigo)
        {
            if (!ReservaCancelacionHelper.EsMotivoValido(motivoCodigo))
            {
                return Json(new { ok = false, error = "Motivo de cancelacion invalido." });
            }

            var reserva = await _context.SolicitudesReserva
                .Include(r => r.EstadoReserva)
                .FirstOrDefaultAsync(r => r.ReservaID == reservaId && r.TecnicoID == TecnicoId);

            if (reserva == null)
            {
                return Json(new { ok = false, error = "Esa reserva no esta asignada a tu cuenta." });
            }

            var estadosCancelables = new[] { "SOLICITADA", "ASIGNADA", "EN_CAMINO" };
            if (!estadosCancelables.Contains(reserva.EstadoReserva?.Codigo))
            {
                return Json(new { ok = false, error = "Esta reserva ya no se puede cancelar desde su estado actual." });
            }

            try
            {
                var sinPenalizacion = await ReservaCancelacionHelper.EstaDentroDeVentanaDeGraciaAsync(_context, reserva.ReservaID);

                var estadoCanceladaId = await _context.EstadosReserva
                    .Where(e => e.Codigo == "CANCELADA")
                    .Select(e => e.EstadoReservaID)
                    .FirstOrDefaultAsync();

                var pReserva = new SqlParameter("@ReservaID", reserva.ReservaID);
                var pEstado = new SqlParameter("@EstadoNuevoID", estadoCanceladaId);
                var pUsuario = new SqlParameter("@UsuarioModificadorID", TecnicoId);
                var pObs = new SqlParameter("@Observaciones", "Cancelado por el tecnico.");

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_Reserva_CambiarEstado @ReservaID, @EstadoNuevoID, @UsuarioModificadorID, @Observaciones",
                    pReserva, pEstado, pUsuario, pObs);

                reserva.MotivoCancelacionCodigo = motivoCodigo;
                reserva.CanceladoPor = "TECNICO";
                reserva.CancelacionConPenalizacion = !sinPenalizacion;

                var estadoOfertaCanceladaId = await _context.EstadosOfertaTecnico
                    .Where(e => e.Codigo == "CANCELADA")
                    .Select(e => e.EstadoOfertaID)
                    .FirstOrDefaultAsync();

                var ofertasAbiertas = await _context.OfertasTecnico
                    .Include(o => o.EstadoOferta)
                    .Where(o => o.ReservaID == reserva.ReservaID &&
                                (o.EstadoOferta!.Codigo == "PENDIENTE" || o.EstadoOferta!.Codigo == "ACEPTADA"))
                    .ToListAsync();

                foreach (var oferta in ofertasAbiertas)
                {
                    oferta.EstadoOfertaID = estadoOfertaCanceladaId;
                    oferta.FechaRespuesta ??= DateTime.Now;
                }

                await _context.SaveChangesAsync();

                // Al cliente le llega el cambio de estado por el mismo
                // evento que ya escucha en Paso6ConfirmacionExitosa.cshtml.
                await _hub.Clients.Group(NotificacionesHub.GrupoReserva(reserva.ReservaID))
                    .SendAsync("EstadoActualizado", new { estado = "CANCELADA" });

                return Json(new { ok = true, conPenalizacion = !sinPenalizacion });
            }
            catch (Exception ex)
            {
                var mensaje = ex.InnerException?.Message ?? ex.Message;
                var errorMsg = mensaje.Contains("Transicion de estado no permitida")
                    ? "Esta reserva ya no se puede cancelar desde su estado actual."
                    : "No se pudo cancelar la reserva: " + mensaje;
                return Json(new { ok = false, error = errorMsg });
            }
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
