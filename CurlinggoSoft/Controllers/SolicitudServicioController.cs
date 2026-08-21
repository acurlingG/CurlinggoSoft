using CurlinggoSoft.Models;
using CurlinggoSoft.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurlinggoSoft.Controllers
{
    // Controlador dedicado al flujo (wizard) para que el cliente solicite un servicio.
    public class SolicitudServicioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDispatchEngineService _dispatchEngine;
        private readonly UserManager<IdentityUser> _userManager;

        // Inyectamos el contexto, el motor de asignación y UserManager (para leer
        // Email/UserName del usuario logueado y poder crear su fila en dbo.Usuarios).
        public SolicitudServicioController(ApplicationDbContext context, IDispatchEngineService dispatchEngine,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _dispatchEngine = dispatchEngine;
            _userManager = userManager;
        }

        // GET: /SolicitudServicio/Paso1Servicio
        [HttpGet]
        public async Task<IActionResult> Paso1Servicio()
        {
            HttpContext.Session.Remove("SubcategoriaID");

            var categorias = await _context.CategoriasServicio
                .Where(c => c.Activa)
                .OrderBy(c => c.NombreCategoria)
                .ToListAsync();

            return View(categorias);
        }

        // POST: /SolicitudServicio/SeleccionarServicio
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SeleccionarServicio(int categoriaId)
        {
            if (categoriaId <= 0)
            {
                TempData["Error"] = "Debes seleccionar un servicio para continuar.";
                return RedirectToAction(nameof(Paso1Servicio));
            }

            HttpContext.Session.SetInt32("CategoriaID", categoriaId);
            return RedirectToAction("Paso2Subcategoria", "SolicitudServicio", new { categoriaId });
        }

        // GET: /SolicitudServicio/Paso2Subcategoria?categoriaId=5
        [HttpGet]
        public async Task<IActionResult> Paso2Subcategoria(int? categoriaId)
        {
            var catId = categoriaId ?? HttpContext.Session.GetInt32("CategoriaID");

            if (catId == null || catId <= 0)
            {
                TempData["Error"] = "Primero debes escoger un servicio.";
                return RedirectToAction(nameof(Paso1Servicio));
            }

            var categoria = await _context.CategoriasServicio.FindAsync(catId.Value);
            if (categoria == null || !categoria.Activa)
            {
                TempData["Error"] = "El servicio seleccionado ya no está disponible.";
                return RedirectToAction(nameof(Paso1Servicio));
            }

            HttpContext.Session.SetInt32("CategoriaID", catId.Value);
            HttpContext.Session.Remove("SubcategoriaID");

            var subcategorias = await _context.SubcategoriasServicio
                .Where(s => s.CategoriaID == catId && s.Activa)
                .OrderBy(s => s.NombreSubcategoria)
                .ToListAsync();

            ViewBag.NombreCategoria = categoria.NombreCategoria;
            ViewBag.CategoriaID = catId.Value;
            return View(subcategorias);
        }

        // POST: /SolicitudServicio/SeleccionarSubcategoria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SeleccionarSubcategoria(int subcategoriaId, int categoriaId)
        {
            if (subcategoriaId <= 0)
            {
                TempData["Error"] = "Debes seleccionar una opción para continuar.";
                return RedirectToAction(nameof(Paso2Subcategoria), new { categoriaId });
            }

            HttpContext.Session.SetInt32("SubcategoriaID", subcategoriaId);
            return RedirectToAction("Paso3Calendario", "SolicitudServicio");
        }

        // POST: /SolicitudServicio/OmitirSubcategoria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OmitirSubcategoria()
        {
            HttpContext.Session.Remove("SubcategoriaID");
            return RedirectToAction("Paso3Calendario", "SolicitudServicio");
        }

        // GET: /SolicitudServicio/Paso3Calendario
        [HttpGet]
        public IActionResult Paso3Calendario()
        {
            var catId = HttpContext.Session.GetInt32("CategoriaID");
            if (catId == null || catId <= 0)
            {
                TempData["Error"] = "Primero debes escoger un servicio.";
                return RedirectToAction(nameof(Paso1Servicio));
            }

            ViewBag.Franjas = FranjasHorarias;
            return View();
        }

        // POST: /SolicitudServicio/SeleccionarHorario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SeleccionarHorario(DateOnly fecha, string horaInicio, string horaFin)
        {
            if (fecha < DateOnly.FromDateTime(DateTime.Today))
            {
                TempData["Error"] = "Escoge una fecha válida (no puede ser en el pasado).";
                return RedirectToAction(nameof(Paso3Calendario));
            }

            if (string.IsNullOrWhiteSpace(horaInicio) || string.IsNullOrWhiteSpace(horaFin))
            {
                TempData["Error"] = "Debes seleccionar una franja de horario.";
                return RedirectToAction(nameof(Paso3Calendario));
            }

            HttpContext.Session.SetString("FechaProgramada", fecha.ToString("yyyy-MM-dd"));
            HttpContext.Session.SetString("HoraInicio", horaInicio);
            HttpContext.Session.SetString("HoraFin", horaFin);

            return RedirectToAction("Paso4Direccion", "SolicitudServicio");
        }

        // GET: /SolicitudServicio/Paso4Direccion
        [HttpGet]
        public async Task<IActionResult> Paso4Direccion()
        {
            var fecha = HttpContext.Session.GetString("FechaProgramada");
            if (string.IsNullOrEmpty(fecha))
            {
                TempData["Error"] = "Primero debes escoger fecha y horario.";
                return RedirectToAction(nameof(Paso3Calendario));
            }

            var provincias = await _context.Provincias
                .Where(p => p.Activa)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            ViewBag.Provincias = provincias;
            return View();
        }

        // GET: /SolicitudServicio/GetCantones?provinciaId=1
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

        // GET: /SolicitudServicio/GetDistritos?cantonId=101
        [HttpGet]
        public async Task<IActionResult> GetDistritos(int cantonId)
        {
            var distritos = await _context.Distritos
                .Where(d => d.CantonID == cantonId && d.Activo)
                .OrderBy(d => d.Nombre)
                .Select(d => new { id = d.DistritoID, nombre = d.Nombre })
                .ToListAsync();

            return Json(distritos);
        }

        // POST: /SolicitudServicio/SeleccionarDireccion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SeleccionarDireccion(int provinciaId, int cantonId, int distritoId,
            string direccionExacta, string nombreContacto, string whatsapp,
            string? latitud, string? longitud)
        {
            if (provinciaId <= 0 || cantonId <= 0 || distritoId <= 0)
            {
                TempData["Error"] = "Debes escoger provincia, cantón y distrito.";
                return RedirectToAction(nameof(Paso4Direccion));
            }

            if (string.IsNullOrWhiteSpace(direccionExacta) ||
                string.IsNullOrWhiteSpace(nombreContacto) ||
                string.IsNullOrWhiteSpace(whatsapp))
            {
                TempData["Error"] = "Completa la dirección exacta, tu nombre y un número de WhatsApp.";
                return RedirectToAction(nameof(Paso4Direccion));
            }

            // Coordenadas capturadas por JS en el Paso 4. Se reciben como STRING
            // a propósito, no como decimal?: el model binding automático de MVC
            // usa la cultura regional del servidor para parsear decimales, y este
            // servidor corre en pt-BR (coma como separador decimal, punto como
            // separador de miles). El navegador manda "9.838721" con punto — bajo
            // pt-BR ese punto se lee como separador de miles y se descarta,
            // convirtiendo la latitud en el entero 9838721 (el bug real detrás
            // del error "Valor de parâmetro está fora do intervalo"). Parseamos
            // manualmente con InvariantCulture para evitar ese problema.
            decimal? latParsed = null, lonParsed = null;
            if (decimal.TryParse(latitud, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var latVal) &&
                decimal.TryParse(longitud, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var lonVal))
            {
                latParsed = latVal;
                lonParsed = lonVal;
            }

            HttpContext.Session.SetInt32("ProvinciaID", provinciaId);
            HttpContext.Session.SetInt32("CantonID", cantonId);
            HttpContext.Session.SetInt32("DistritoID", distritoId);
            HttpContext.Session.SetString("DireccionExacta", direccionExacta.Trim());
            HttpContext.Session.SetString("NombreContacto", nombreContacto.Trim());
            HttpContext.Session.SetString("WhatsApp", whatsapp.Trim());

            if (latParsed.HasValue && lonParsed.HasValue)
            {
                HttpContext.Session.SetString("LatitudServicio", latParsed.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                HttpContext.Session.SetString("LongitudServicio", lonParsed.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            else
            {
                HttpContext.Session.Remove("LatitudServicio");
                HttpContext.Session.Remove("LongitudServicio");
            }

            return RedirectToAction("Paso5Resumen", "SolicitudServicio");
        }

        // GET: /SolicitudServicio/Paso5Resumen
        [HttpGet]
        public async Task<IActionResult> Paso5Resumen()
        {
            var categoriaId = HttpContext.Session.GetInt32("CategoriaID");
            var fecha = HttpContext.Session.GetString("FechaProgramada");
            var provinciaId = HttpContext.Session.GetInt32("ProvinciaID");

            if (categoriaId == null || string.IsNullOrEmpty(fecha) || provinciaId == null)
            {
                TempData["Error"] = "Tu sesión ha expirado o faltan pasos por completar.";
                return RedirectToAction(nameof(Paso1Servicio));
            }

            var subcategoriaId = HttpContext.Session.GetInt32("SubcategoriaID");

            var servicio = await _context.Servicios
                .Include(s => s.Categoria)
                .Include(s => s.Subcategoria)
                .FirstOrDefaultAsync(s => s.CategoriaID == categoriaId &&
                                          (subcategoriaId == null || s.SubcategoriaID == subcategoriaId) &&
                                          s.Activo);

            if (servicio == null)
            {
                TempData["Error"] = "El servicio seleccionado no está disponible.";
                return RedirectToAction(nameof(Paso1Servicio));
            }

            var provincia = await _context.Provincias.FindAsync(provinciaId);
            var canton = await _context.Cantones.FindAsync(HttpContext.Session.GetInt32("CantonID"));
            var distrito = await _context.Distritos.FindAsync(HttpContext.Session.GetInt32("DistritoID"));

            HttpContext.Session.SetInt32("ServicioID", servicio.ServicioID);

            ViewBag.ServicioNombre = servicio.NombreServicio;
            ViewBag.CategoriaNombre = servicio.Categoria?.NombreCategoria;
            ViewBag.Fecha = fecha;
            ViewBag.HoraInicio = HttpContext.Session.GetString("HoraInicio") ?? "";
            ViewBag.HoraFin = HttpContext.Session.GetString("HoraFin") ?? "";
            ViewBag.UbicacionTexto = $"{provincia?.Nombre}, {canton?.Nombre}, {distrito?.Nombre}";
            ViewBag.DireccionExacta = HttpContext.Session.GetString("DireccionExacta") ?? "";
            ViewBag.NombreContacto = HttpContext.Session.GetString("NombreContacto") ?? "";
            ViewBag.WhatsApp = HttpContext.Session.GetString("WhatsApp") ?? "";
            ViewBag.MontoBase = servicio.TarifaDiagnosticoBase;
            ViewBag.Iva = servicio.TarifaDiagnosticoBase * 0.13m;
            ViewBag.MontoTotal = servicio.TarifaDiagnosticoBase * 1.13m;

            return View();
        }

        // POST: /SolicitudServicio/ConfirmarYPagar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarYPagar(string descripcionProblema, string metodoPago, string comprobante)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Paso5Resumen", "SolicitudServicio") });
            }

            var clienteId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(clienteId))
            {
                TempData["Error"] = "No se pudo identificar la cuenta de usuario.";
                return RedirectToAction("Paso5Resumen");
            }

            if (string.IsNullOrWhiteSpace(descripcionProblema))
            {
                TempData["Error"] = "Por favor indica una breve descripción del problema.";
                return RedirectToAction("Paso5Resumen");
            }

            var provinciaId = HttpContext.Session.GetInt32("ProvinciaID");
            var cantonId = HttpContext.Session.GetInt32("CantonID");
            var distritoId = HttpContext.Session.GetInt32("DistritoID");
            var direccionExacta = HttpContext.Session.GetString("DireccionExacta");
            var servicioId = HttpContext.Session.GetInt32("ServicioID");

            if (provinciaId == null || cantonId == null || distritoId == null || servicioId == null)
            {
                TempData["Error"] = "La sesión expiró. Por favor inicia la solicitud nuevamente.";
                return RedirectToAction(nameof(Paso1Servicio));
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // FK_ClientesPerfil_Usuarios exige que ya exista una fila en dbo.Usuarios
                // con este mismo Id. Como AspNetUsers (Identity) y dbo.Usuarios son tablas
                // separadas sin sincronización automática, la creamos aquí si falta.
                await AsegurarUsuarioAsync(clienteId);

                // Entidad ClientePerfil en singular
                var perfilCliente = await _context.ClientesPerfil.FindAsync(clienteId);
                if (perfilCliente == null)
                {
                    perfilCliente = new ClientePerfil
                    {
                        ClienteID = clienteId,
                        ProvinciaID = provinciaId.Value,
                        CantonID = cantonId.Value,
                        DistritoID = distritoId.Value,
                        DireccionExacta = direccionExacta ?? "",
                        FechaActualizacion = DateTime.Now
                    };
                    _context.ClientesPerfil.Add(perfilCliente);
                    await _context.SaveChangesAsync();
                }

                // Buscamos si el cliente ya tiene una dirección guardada con ese mismo
                // nombre (UQ_Direcciones_Cliente_Nombre exige ClienteID+NombreDireccion
                // único). Si existe, la reusamos/actualizamos en vez de insertar de
                // nuevo; si no existe, la creamos. Esto evita el choque de llave
                // duplicada cuando el mismo cliente hace una segunda solicitud.
                const string nombreDireccionServicio = "Dirección de Servicio";

                var nuevaDireccion = await _context.DireccionesCliente
                    .FirstOrDefaultAsync(d => d.ClienteID == clienteId &&
                                               d.NombreDireccion == nombreDireccionServicio);

                if (nuevaDireccion == null)
                {
                    nuevaDireccion = new DireccionCliente
                    {
                        ClienteID = clienteId,
                        NombreDireccion = nombreDireccionServicio,
                        ProvinciaID = provinciaId.Value,
                        CantonID = cantonId.Value,
                        DistritoID = distritoId.Value,
                        DireccionExacta = direccionExacta ?? "",
                        EsPrincipal = false,
                        Activa = true,
                        FechaCreacion = DateTime.Now
                    };
                    _context.DireccionesCliente.Add(nuevaDireccion);
                }
                else
                {
                    // Actualizamos con los datos de esta solicitud, ya que la
                    // dirección exacta pudo haber cambiado desde la última vez.
                    nuevaDireccion.ProvinciaID = provinciaId.Value;
                    nuevaDireccion.CantonID = cantonId.Value;
                    nuevaDireccion.DistritoID = distritoId.Value;
                    nuevaDireccion.DireccionExacta = direccionExacta ?? "";
                    nuevaDireccion.Activa = true;
                }

                await _context.SaveChangesAsync();

                var estadoSolicitada = await _context.EstadosReserva.FirstOrDefaultAsync(e => e.Codigo == "SOLICITADA");
                var servicio = await _context.Servicios.FindAsync(servicioId.Value);

                if (estadoSolicitada == null)
                {
                    throw new InvalidOperationException(
                        "No existe un EstadoReserva con Codigo='SOLICITADA'. Revisa el seed de dbo.EstadosReserva.");
                }
                if (servicio == null)
                {
                    throw new InvalidOperationException(
                        $"El ServicioID={servicioId.Value} de la sesión ya no existe o está inactivo.");
                }

                var fechaStr = HttpContext.Session.GetString("FechaProgramada");
                var horaInicioStr = HttpContext.Session.GetString("HoraInicio");
                DateTime fechaHoraProgramada = DateTime.Parse($"{fechaStr} {horaInicioStr}");

                // Coordenadas capturadas en el Paso 4. Si el cliente negó el
                // permiso de ubicación, estas quedan null — eso es esperado y NO
                // debe bloquear la reserva. Lo que sí hace es que
                // usp_Reserva_BuscarTecnicosDisponibles no va a poder calcular
                // distancia (columna calculada UbicacionGeoServicio queda NULL),
                // así que el dispatch por radio geográfico fallará más adelante.
                // TODO: cuando se implemente el fallback por zona
                // (ProvinciaCoberturaID/CantonCoberturaID de TecnicosPerfil), este
                // es el punto donde decidir cuál camino tomar.
                decimal? latitudServicio = null;
                decimal? longitudServicio = null;
                var latStr = HttpContext.Session.GetString("LatitudServicio");
                var lonStr = HttpContext.Session.GetString("LongitudServicio");
                if (decimal.TryParse(latStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var latParsed) &&
                    decimal.TryParse(lonStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var lonParsed))
                {
                    latitudServicio = latParsed;
                    longitudServicio = lonParsed;
                }

                // Entidad SolicitudReserva en singular
                var reserva = new SolicitudReserva
                {
                    CodigoSeguimiento = Guid.NewGuid(),
                    ClienteID = clienteId,
                    ServicioID = servicioId.Value,
                    EstadoReservaID = estadoSolicitada.EstadoReservaID,
                    DireccionID = nuevaDireccion.DireccionID,
                    ProvinciaID = provinciaId,
                    CantonID = cantonId,
                    DistritoID = distritoId,
                    LatitudServicio = latitudServicio,
                    LongitudServicio = longitudServicio,
                    MontoBaseCotizado = servicio.TarifaDiagnosticoBase,
                    MontoAjustes = 0,
                    MontoTotalCotizado = servicio.TarifaDiagnosticoBase * 1.13m,
                    Moneda = servicio.Moneda,
                    DuracionEstimadaMinutos = servicio.TiempoEstimadoMinutos,
                    FechaHoraProgramada = fechaHoraProgramada,
                    FechaHoraSolicitud = DateTime.Now,
                    DireccionServicio = direccionExacta ?? "",
                    DescripcionProblema = descripcionProblema.Trim()
                };

                _context.SolicitudesReserva.Add(reserva);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                HttpContext.Session.Remove("CategoriaID");
                HttpContext.Session.Remove("SubcategoriaID");
                HttpContext.Session.Remove("FechaProgramada");
                HttpContext.Session.Remove("HoraInicio");
                HttpContext.Session.Remove("HoraFin");
                HttpContext.Session.Remove("LatitudServicio");
                HttpContext.Session.Remove("LongitudServicio");

                // DISPARAR EL MOTOR DE ASIGNACIÓN (Pulse / Uber Style)
                // Se envía la alerta a los 3 mejores técnicos de la zona en este instante.
                // IMPORTANTE: esto va FUERA del try/catch transaccional. La reserva ya
                // quedó guardada (commit exitoso arriba); si el dispatch engine falla
                // (ej. no hay técnicos disponibles en la zona), no debe:
                //   a) hacer rollback de una transacción que ya se comprometió
                //      (eso causa el error "This SqlTransaction has completed" /
                //      "Este SqlTransaction foi concluído" que estabas viendo), ni
                //   b) mostrarle al cliente un error como si su reserva no se hubiera
                //      creado, cuando en realidad sí se creó.
                // Si falla, lo registramos pero igual mandamos al cliente a la
                // pantalla de confirmación; el reintento de asignación se puede
                // manejar con un job en background más adelante.
                try
                {
                    await _dispatchEngine.GenerarOfertasLoteInicialAsync(reserva.ReservaID, tamanoLote: 3);
                }
                catch (Exception dispatchEx)
                {
                    // TODO: reemplazar por ILogger<SolicitudServicioController> cuando
                    // lo tengas inyectado. Por ahora al menos no perdemos el error.
                    System.Diagnostics.Debug.WriteLine(
                        $"[DispatchEngine] Falló la asignación inicial para ReservaID={reserva.ReservaID}: {dispatchEx}");
                }

                return RedirectToAction("Paso6ConfirmacionExitosa", new { id = reserva.ReservaID });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // ex.Message solo trae el mensaje genérico de EF Core ("...See the inner
                // exception..."). El motivo real (constraint, columna, etc.) vive en InnerException.
                var mensajeReal = ex.InnerException?.Message ?? ex.Message;
                TempData["Error"] = "Ocurrió un error al procesar la reserva: " + mensajeReal;
                return RedirectToAction("Paso5Resumen");
            }
        }

        // FK_ClientesPerfil_Usuarios exige que exista una fila en dbo.Usuarios con el mismo Id
        // que en AspNetUsers. Como no hay sincronización automática entre ambas tablas, la
        // creamos (o completamos, si faltan Nombre/Apellidos) justo antes de usarla.
        private async Task AsegurarUsuarioAsync(string usuarioId)
        {
            var yaExiste = await _context.Usuarios.FindAsync(usuarioId);
            if (yaExiste != null) return;

            var identityUser = await _userManager.FindByIdAsync(usuarioId);
            var email = identityUser?.Email ?? identityUser?.UserName ?? $"{usuarioId}@curlinggo.local";

            _context.Usuarios.Add(new Usuario
            {
                UsuarioID = usuarioId,
                Email = email,
                // TODO: si más adelante capturas Nombre/Apellidos reales en el registro,
                // reemplaza este placeholder por los datos verdaderos del cliente.
                Nombre = identityUser?.UserName ?? "Cliente",
                Apellidos = "",
                EstadoUsuario = "ACTIVO",
                FechaCreacion = DateTime.Now
            });
            await _context.SaveChangesAsync();
        }

        // GET: /SolicitudServicio/Paso6ConfirmacionExitosa/5
        [HttpGet]
        public async Task<IActionResult> Paso6ConfirmacionExitosa(long id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var clienteId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var reserva = await _context.SolicitudesReserva
                .Include(r => r.Servicio)
                .FirstOrDefaultAsync(r => r.ReservaID == id && r.ClienteID == clienteId);

            if (reserva == null)
            {
                TempData["Error"] = "No se encontró la orden solicitada.";
                return RedirectToAction("Index", "Home");
            }

            return View(reserva);
        }

        private static readonly List<FranjaHoraria> FranjasHorarias = GenerarFranjasHorarias(7, 21);

        private static List<FranjaHoraria> GenerarFranjasHorarias(int horaInicio, int horaFin)
        {
            var franjas = new List<FranjaHoraria>();
            for (int h = horaInicio; h < horaFin; h++)
            {
                var inicio = new TimeOnly(h, 0);
                var fin = new TimeOnly(h + 1, 0);
                var etiqueta = $"{inicio:HH:mm} - {fin:HH:mm}";
                franjas.Add(new FranjaHoraria(etiqueta, inicio.ToString("HH:mm"), fin.ToString("HH:mm")));
            }
            return franjas;
        }
    }

    public record FranjaHoraria(string Etiqueta, string HoraInicio, string HoraFin);
}