using CurlinggoSoft.Models;
using CurlinggoSoft.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurlinggoSoft.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Admin
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Admin/ObtenerMetricasDashboard
        // Devuelve en un solo JSON todas las metricas operativas que
        // consume Views/Admin/Index.cshtml para el panel en tiempo real.
        [HttpGet]
        public async Task<IActionResult> ObtenerMetricasDashboard()
        {
            var ahora = DateTime.Now;

            // --- KPIs principales ---

            var estadosActivosCodigos = new[] { "ASIGNADA", "EN_CAMINO", "EN_PROCESO" };
            var reservasActivas = await _context.SolicitudesReserva
                .Include(r => r.EstadoReserva)
                .CountAsync(r => estadosActivosCodigos.Contains(r.EstadoReserva!.Codigo));

            var tecnicosTotales = await _context.TecnicosPerfil.CountAsync();
            var tecnicosConectados = await _context.TecnicosPerfil.CountAsync(t => t.Disponible);

            var clientesIds = await _userManager.GetUsersInRoleAsync("Cliente");
            var clientesRegistrados = clientesIds.Count;

            // Promedio de solicitudes por dia: total de reservas dividido entre
            // los dias transcurridos desde la primera solicitud registrada.
            var primeraSolicitud = await _context.SolicitudesReserva
                .OrderBy(r => r.FechaHoraSolicitud)
                .Select(r => (DateTime?)r.FechaHoraSolicitud)
                .FirstOrDefaultAsync();

            var totalReservas = await _context.SolicitudesReserva.CountAsync();
            double promedioSolicitudesDia;
            if (primeraSolicitud == null)
            {
                promedioSolicitudesDia = 0;
            }
            else
            {
                var diasTranscurridos = Math.Max(1, (ahora - primeraSolicitud.Value).TotalDays);
                promedioSolicitudesDia = Math.Round(totalReservas / diasTranscurridos, 1);
            }

            // Tiempo promedio de match: minutos desde que se crea la reserva
            // (FechaHoraSolicitud) hasta que el tecnico acepta la oferta
            // (FechaRespuesta de la oferta ACEPTADA).
            var tiemposMatch = await _context.OfertasTecnico
                .Include(o => o.EstadoOferta)
                .Include(o => o.Reserva)
                .Where(o => o.EstadoOferta!.Codigo == "ACEPTADA" && o.FechaRespuesta != null && o.Reserva != null)
                .Select(o => new { o.FechaRespuesta, o.Reserva!.FechaHoraSolicitud })
                .ToListAsync();

            double tiempoPromedioMatch = tiemposMatch.Count == 0
                ? 0
                : Math.Round(tiemposMatch.Average(t => (t.FechaRespuesta!.Value - t.FechaHoraSolicitud).TotalMinutes), 1);

            // Tasa de exito de match: reservas con tecnico asignado (TecnicoID
            // no nulo) frente al total de reservas (huerfanas = sin tecnico).
            var reservasConTecnico = await _context.SolicitudesReserva.CountAsync(r => r.TecnicoID != null);
            double tasaExitoMatch = totalReservas == 0
                ? 0
                : Math.Round((double)reservasConTecnico / totalReservas * 100, 1);

            // --- Distribucion operativa ---

            var conteosPorEstado = await _context.SolicitudesReserva
                .Include(r => r.EstadoReserva)
                .GroupBy(r => r.EstadoReserva!.Codigo)
                .Select(g => new { Codigo = g.Key, Cantidad = g.Count() })
                .ToListAsync();

            int ObtenerConteo(string codigo) =>
                conteosPorEstado.FirstOrDefault(e => e.Codigo == codigo)?.Cantidad ?? 0;

            var estados = new
            {
                solicitada = ObtenerConteo("SOLICITADA"),
                asignada = ObtenerConteo("ASIGNADA"),
                enCamino = ObtenerConteo("EN_CAMINO"),
                enProceso = ObtenerConteo("EN_PROCESO")
            };

            // Tecnicos conectados por nivel de match del motor de despacho:
            //  Nivel 1: GPS en vivo (tiene registro en TecnicosUbicacionActual)
            //  Nivel 2: sin GPS pero con ubicacion de referencia (lat/lon del perfil)
            //  Nivel 3: solo zona de cobertura (provincia/canton)
            var tecnicosDisponibles = await _context.TecnicosPerfil
                .Where(t => t.Disponible)
                .Select(t => new
                {
                    t.TecnicoID,
                    t.LatitudActual,
                    t.LongitudActual,
                    t.ProvinciaCoberturaID,
                    t.CantonCoberturaID
                })
                .ToListAsync();

            var tecnicosConGpsVivo = await _context.TecnicosUbicacionActual
                .Select(u => u.TecnicoID)
                .ToListAsync();
            var setGpsVivo = tecnicosConGpsVivo.ToHashSet();

            int gpsEnVivo = 0, ubicacionReferencia = 0, zonaCobertura = 0;
            foreach (var tecnico in tecnicosDisponibles)
            {
                if (setGpsVivo.Contains(tecnico.TecnicoID))
                {
                    gpsEnVivo++;
                }
                else if (tecnico.LatitudActual != null && tecnico.LongitudActual != null)
                {
                    ubicacionReferencia++;
                }
                else if (tecnico.ProvinciaCoberturaID != null || tecnico.CantonCoberturaID != null)
                {
                    zonaCobertura++;
                }
            }

            var nivelesMatch = new
            {
                gpsEnVivo,
                ubicacionReferencia,
                zonaCobertura
            };

            // --- Control de calidad y auditoria ---

            var motivosConteo = await _context.SolicitudesReserva
                .Where(r => r.MotivoCancelacionCodigo != null)
                .GroupBy(r => r.MotivoCancelacionCodigo)
                .Select(g => new { Motivo = g.Key!, Cantidad = g.Count() })
                .ToListAsync();

            var totalCancelaciones = motivosConteo.Sum(m => m.Cantidad);
            var motivosCancelacion = ReservaCancelacionHelper.MotivosCancelacionPermitidos
                .Select(m =>
                {
                    var cantidad = motivosConteo.FirstOrDefault(x => x.Motivo == m.Key)?.Cantidad ?? 0;
                    var porcentaje = totalCancelaciones == 0 ? 0 : Math.Round((double)cantidad / totalCancelaciones * 100, 1);
                    return new
                    {
                        codigo = m.Key,
                        etiqueta = m.Value,
                        cantidad,
                        porcentaje
                    };
                })
                .OrderByDescending(m => m.cantidad)
                .ToList();

            var logsRecientes = await _context.LogsIntervencionOperativa
                .OrderByDescending(l => l.FechaRegistro)
                .Take(15)
                .Select(l => new
                {
                    l.TipoEvento,
                    l.DecisionTomada,
                    l.FechaRegistro
                })
                .ToListAsync();

            var alertasTipos = new[] { "CANCELACION_TARDIA", "RE_DISPATCH", "ALERTA" };
            var logs = logsRecientes.Select(l => new
            {
                mensaje = $"[{l.TipoEvento}] {l.DecisionTomada}",
                tiempoAtras = FormatearTiempoTranscurrido(ahora - l.FechaRegistro),
                esAlerta = alertasTipos.Any(a => l.TipoEvento.Contains(a, StringComparison.OrdinalIgnoreCase))
            });

            return Json(new
            {
                reservasActivas,
                tecnicosConectados,
                tecnicosTotales,
                clientesRegistrados,
                promedioSolicitudesDia,
                tiempoPromedioMatch,
                tasaExitoMatch,
                estados,
                nivelesMatch,
                motivosCancelacion,
                logsRecientes = logs
            });
        }

        private static string FormatearTiempoTranscurrido(TimeSpan transcurrido)
        {
            if (transcurrido.TotalMinutes < 1) return "hace instantes";
            if (transcurrido.TotalMinutes < 60) return $"hace {(int)transcurrido.TotalMinutes} min";
            if (transcurrido.TotalHours < 24) return $"hace {(int)transcurrido.TotalHours} h";
            return $"hace {(int)transcurrido.TotalDays} d";
        }
    }
}