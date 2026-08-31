using System.Security.Claims;
using CURLINGgo.API.DTOs;
using CurlinggoSoft.Models;
using CurlinggoSoft.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CURLINGgo.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDispatchEngineService _dispatchEngine;

        public SolicitudesController(ApplicationDbContext context, IDispatchEngineService dispatchEngine)
        {
            _context = context;
            _dispatchEngine = dispatchEngine;
        }

        // GET: api/solicitudes/mis-solicitudes
        [HttpGet("mis-solicitudes")]
        public async Task<IActionResult> GetMisSolicitudes()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var cliente = await _context.ClientesPerfil.FirstOrDefaultAsync(c => c.ClienteID == userId);
            if (cliente == null)
                return NotFound(new { mensaje = "Perfil de cliente no encontrado." });

            var solicitudes = await _context.SolicitudesReserva
                .Where(s => s.ClienteID == cliente.ClienteID)
                .OrderByDescending(s => s.FechaHoraProgramada)
                .Select(s => new SolicitudResumenDto
                {
                    ReservaID = s.ReservaID,
                    CodigoSeguimiento = s.CodigoSeguimiento,
                    ServicioNombre = s.Servicio != null ? s.Servicio.NombreServicio : "Servicio",
                    FechaHoraProgramada = s.FechaHoraProgramada,
                    EstadoReservaID = s.EstadoReservaID,
                    EstadoNombre = s.EstadoReserva != null ? s.EstadoReserva.Nombre : "Pendiente",
                    DireccionServicio = s.DireccionServicio,
                    MontoTotalCotizado = s.MontoTotalCotizado,
                    TecnicoID = s.TecnicoID
                })
                .ToListAsync();

            return Ok(solicitudes);
        }

        // POST: api/solicitudes
        [HttpPost]
        public async Task<IActionResult> CrearSolicitud([FromBody] CrearSolicitudDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var cliente = await _context.ClientesPerfil.FirstOrDefaultAsync(c => c.ClienteID == userId);
            if (cliente == null)
                return BadRequest(new { mensaje = "El usuario actual no tiene perfil de Cliente activo." });

            var servicio = await _context.Servicios.FindAsync(dto.ServicioID);
            if (servicio == null)
                return NotFound(new { mensaje = "El servicio especificado no existe." });

            // Determinar valores válidos para la ubicación
            int provinciaId = (dto.ProvinciaID.HasValue && dto.ProvinciaID > 0) ? dto.ProvinciaID.Value : (cliente.ProvinciaID > 0 ? cliente.ProvinciaID : 1);
            int cantonId = (dto.CantonID.HasValue && dto.CantonID > 0) ? dto.CantonID.Value : (cliente.CantonID > 0 ? cliente.CantonID : 1);
            int distritoId = (dto.DistritoID.HasValue && dto.DistritoID > 0) ? dto.DistritoID.Value : (cliente.DistritoID > 0 ? cliente.DistritoID : 1);

            var solicitud = new SolicitudReserva
            {
                ClienteID = cliente.ClienteID,
                ServicioID = dto.ServicioID,
                DireccionID = dto.DireccionID, // AGREGAR: Asignación explícita para cumplir con CK_Reservas_DireccionCompleta
                EstadoReservaID = 1, // ID Estado Pendiente por defecto
                FechaHoraProgramada = dto.FechaHoraProgramada,
                FechaHoraSolicitud = DateTime.Now,
                DireccionServicio = string.IsNullOrWhiteSpace(dto.DireccionServicio) ? "Dirección predeterminada" : dto.DireccionServicio,

                ProvinciaID = provinciaId,
                CantonID = cantonId,
                DistritoID = distritoId,

                DescripcionProblema = string.IsNullOrWhiteSpace(dto.DescripcionProblema) ? "Sin descripción detallada" : dto.DescripcionProblema,
                LatitudServicio = dto.LatitudServicio,
                LongitudServicio = dto.LongitudServicio,
                NotasCliente = dto.NotasCliente,
                MontoBaseCotizado = servicio.TarifaDiagnosticoBase,
                MontoAjustes = 0,
                MontoTotalCotizado = servicio.TarifaDiagnosticoBase,
                Moneda = servicio.Moneda,
                DuracionEstimadaMinutos = servicio.TiempoEstimadoMinutos
            };

            _context.SolicitudesReserva.Add(solicitud);
            await _context.SaveChangesAsync();

            // Ejecutar el motor de despacho de ofertas iniciales
            try
            {
                await _dispatchEngine.GenerarOfertasLoteInicialAsync(solicitud.ReservaID);
            }
            catch
            {
                // Evita interrumpir la creación de la reserva si no hay técnicos disponibles
            }

            return CreatedAtAction(nameof(GetMisSolicitudes), new { id = solicitud.ReservaID }, solicitud);
        }

        // GET: api/solicitudes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSolicitudPorId(long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var solicitud = await _context.SolicitudesReserva
                .AsNoTracking()
                .Where(s => s.ReservaID == id)
                .Select(s => new SolicitudDetalleDto
                {
                    ReservaID = s.ReservaID,
                    CodigoSeguimiento = s.CodigoSeguimiento,
                    ServicioID = s.ServicioID,
                    ServicioNombre = s.Servicio != null ? s.Servicio.NombreServicio : "Servicio",
                    ClienteID = s.ClienteID,
                    FechaHoraProgramada = s.FechaHoraProgramada,
                    FechaHoraSolicitud = s.FechaHoraSolicitud,
                    EstadoReservaID = s.EstadoReservaID,
                    EstadoNombre = s.EstadoReserva != null ? s.EstadoReserva.Nombre : "Pendiente",
                    DireccionID = s.DireccionID,
                    DireccionServicio = s.DireccionServicio,
                    ProvinciaID = s.ProvinciaID,
                    CantonID = s.CantonID,
                    DistritoID = s.DistritoID,
                    DescripcionProblema = s.DescripcionProblema,
                    LatitudServicio = s.LatitudServicio,
                    LongitudServicio = s.LongitudServicio,
                    NotasCliente = s.NotasCliente,
                    MontoBaseCotizado = s.MontoBaseCotizado,
                    MontoAjustes = s.MontoAjustes,
                    MontoTotalCotizado = s.MontoTotalCotizado,
                    Moneda = s.Moneda,
                    TecnicoID = s.TecnicoID
                })
                .FirstOrDefaultAsync();

            if (solicitud == null)
                return NotFound(new { mensaje = "La solicitud especificada no existe." });

            return Ok(solicitud);
        }

        // PUT: api/solicitudes/{id}/estado
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(long id, [FromBody] CambiarEstadoSolicitudDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var solicitud = await _context.SolicitudesReserva.FindAsync(id);
            if (solicitud == null)
                return NotFound(new { mensaje = "La solicitud especificada no existe." });

            // Validar que solo el cliente o el técnico asignado puedan cambiar el estado
            if (solicitud.ClienteID != userId && solicitud.TecnicoID != userId)
            {
                return StatusCode(403, new { mensaje = "No tiene permisos para modificar el estado de esta reserva." });
            }

            if (solicitud.EstadoReservaID == dto.NuevoEstadoID)
            {
                return BadRequest(new { mensaje = "La reserva ya se encuentra en el estado indicado." });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int estadoAnteriorId = solicitud.EstadoReservaID;

                // 1. Actualizar el estado actual de la reserva
                solicitud.EstadoReservaID = dto.NuevoEstadoID;

                // 2. Registrar la traza en el historial de estados
                var historial = new HistorialEstadoReserva
                {
                    ReservaID = solicitud.ReservaID,
                    EstadoAnteriorID = estadoAnteriorId,
                    EstadoNuevoID = dto.NuevoEstadoID,
                    FechaCambio = DateTime.Now,
                    UsuarioModificadorID = userId,
                    Observaciones = string.IsNullOrWhiteSpace(dto.Observaciones)
                        ? $"Cambio de estado a ID {dto.NuevoEstadoID}"
                        : dto.Observaciones
                };

                _context.HistorialEstadosReserva.Add(historial);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    mensaje = "Estado actualizado correctamente.",
                    reservaId = solicitud.ReservaID,
                    estadoAnterior = estadoAnteriorId,
                    nuevoEstado = dto.NuevoEstadoID
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { mensaje = "Error al actualizar el estado de la reserva.", detalle = ex.Message });
            }
        }
    }
}