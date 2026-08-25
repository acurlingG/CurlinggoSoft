using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;
using CurlinggoSoft.Models.ViewModels;
using System.Security.Claims;

namespace CurlinggoSoft.Controllers
{
    // Wizard de 8 pasos para que un aspirante se registre como técnico.
    // Reglas de negocio:
    //  - Si el usuario ya tiene sesión iniciada (Cliente u otro rol), se
    //    reutiliza su cuenta de Identity/Usuario y el Paso 2 se precarga.
    //  - Si es anónimo, el Paso 2 le permite crear su cuenta (Identity +
    //    Usuario) SIN asignar el rol "Tecnico" todavía; eso solo ocurre
    //    cuando la solicitud es aprobada (usp_SolicitudTecnico_Aprobar).
    //  - El progreso se guarda incrementalmente en SolicitudTecnico para
    //    soportar "Guardar y salir" / retomar más tarde.
    [Authorize]
    public class SolicitudTecnicoController : Controller
    {
        private const string SesionSolicitudKey = "SolicitudTecnicoID_EnProgreso";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public SolicitudTecnicoController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ---------------------------------------------------------------
        // Paso 1: Bienvenida / continuar solicitud existente
        // ---------------------------------------------------------------
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            // Si el usuario está logueado y ya tiene una solicitud "viva"
            // (no rechazada/cancelada), lo mandamos directo a retomarla.
            if (User.Identity?.IsAuthenticated == true)
            {
                var usuarioId = _userManager.GetUserId(User);
                var solicitudExistente = await _context.SolicitudesTecnico
                    .Include(s => s.EstadoSolicitud)
                    .Where(s => s.UsuarioID == usuarioId)
                    .OrderByDescending(s => s.FechaCreacion)
                    .FirstOrDefaultAsync(s => s.EstadoSolicitud!.Codigo != "RECHAZADA" && s.EstadoSolicitud!.Codigo != "CANCELADA");

                if (solicitudExistente != null)
                {
                    HttpContext.Session.SetString(SesionSolicitudKey, solicitudExistente.SolicitudTecnicoID.ToString());
                    return RedirectToAction(nameof(Paso), new { paso = ObtenerPasoSegunEstado(solicitudExistente) });
                }
            }

            return View();
        }

        // POST desde el Paso 1: crea el expediente en BORRADOR y avanza al Paso 2.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comenzar()
        {
            string? usuarioId = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                usuarioId = _userManager.GetUserId(User);
            }

            long solicitudId;

            if (usuarioId != null)
            {
                // Reutiliza cuenta existente; si ya hay solicitud viva, la retoma.
                var solicitud = await _context.SolicitudesTecnico
                    .Include(s => s.EstadoSolicitud)
                    .Where(s => s.UsuarioID == usuarioId)
                    .OrderByDescending(s => s.FechaCreacion)
                    .FirstOrDefaultAsync(s => s.EstadoSolicitud!.Codigo != "RECHAZADA" && s.EstadoSolicitud!.Codigo != "CANCELADA");

                if (solicitud == null)
                {
                    solicitud = await CrearSolicitudBorradorAsync(usuarioId);
                }

                solicitudId = solicitud.SolicitudTecnicoID;
            }
            else
            {
                // Anónimo: la cuenta se crea hasta el Paso 2 (DatosPersonales).
                // Guardamos un marcador vacío en sesión para indicar "en progreso sin cuenta".
                HttpContext.Session.Remove(SesionSolicitudKey);
                return RedirectToAction(nameof(Paso), new { paso = 2 });
            }

            HttpContext.Session.SetString(SesionSolicitudKey, solicitudId.ToString());
            return RedirectToAction(nameof(Paso), new { paso = 2 });
        }

        // ---------------------------------------------------------------
        // Navegación genérica por paso (GET)
        // ---------------------------------------------------------------
        [AllowAnonymous]
        public async Task<IActionResult> Paso(int paso)
        {
            if (paso < 1 || paso > 8) return NotFound();

            var solicitud = await ObtenerSolicitudEnProgresoAsync();
            var modelo = new SolicitudTecnicoWizardViewModel { PasoActual = paso };

            if (solicitud != null)
            {
                modelo.SolicitudTecnicoID = solicitud.SolicitudTecnicoID;
                modelo.CodigoSolicitud = solicitud.CodigoSolicitud;
                await CargarModeloDesdeSolicitudAsync(modelo, solicitud);
            }

            if (paso == 3)
            {
                modelo.Especialidades.Categorias = await ObtenerCategoriasConServiciosAsync();
            }

            if (paso == 5)
            {
                ViewBag.Provincias = await _context.Provincias.Where(p => p.Activa).OrderBy(p => p.Nombre).ToListAsync();
            }

            if (paso == 7)
            {
                var tiposDocumento = await _context.TiposDocumentoTecnico.Where(t => t.Activo).OrderBy(t => t.Nombre).ToListAsync();
                ViewBag.TiposDocumento = tiposDocumento;

                if (solicitud != null)
                {
                    modelo.Documentos.Documentos = tiposDocumento.Select(t =>
                    {
                        var docExistente = solicitud.Documentos.FirstOrDefault(d => d.TipoDocumentoID == t.TipoDocumentoID);
                        return new DocumentoCargaViewModel
                        {
                            TipoDocumentoID = t.TipoDocumentoID,
                            NombreTipoDocumento = t.Nombre,
                            Obligatorio = t.Obligatorio,
                            NombreArchivo = docExistente?.NombreArchivo,
                            RutaArchivo = docExistente?.RutaArchivo
                        };
                    }).ToList();
                }
            }

            return View($"Paso{paso}", modelo);
        }

        // Endpoint AJAX usado por Paso3.cshtml para poblar el <select> de
        // servicios cuando se elige una categoría.
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerServiciosPorCategoria(int categoriaId)
        {
            var categoria = await _context.CategoriasServicio
                .Where(c => c.CategoriaID == categoriaId && c.Activa)
                .Select(c => c.Servicios
                    .Where(s => s.Activo)
                    .OrderBy(s => s.NombreServicio)
                    .Select(s => new { id = s.ServicioID, nombre = s.NombreServicio }))
                .FirstOrDefaultAsync();

            return Json(categoria ?? Enumerable.Empty<object>());
        }

        // ---------------------------------------------------------------
        // Paso 2: Datos Personales
        // ---------------------------------------------------------------
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarPaso2(SolicitudTecnicoWizardViewModel modelo, string? clave)
        {
            // El modelo es único para los 8 pasos, así que el binding trae
            // también los campos de otros pasos (ej. AutorizaBackgroundCheck
            // del Paso 7, ConfirmaInformacionVeridica del Paso 8) que no se
            // envían desde este formulario y llegan en false, disparando sus
            // [Range(...,"true","true")]. Solo validamos lo que pertenece al
            // Paso 2 (DatosPersonales + clave) e ignoramos el resto.
            foreach (var key in ModelState.Keys.Where(k => !k.StartsWith("DatosPersonales") && k != "clave").ToList())
            {
                ModelState.Remove(key);
            }

            if (!ModelState.IsValid)
            {
                modelo.PasoActual = 2;
                return View("Paso2", modelo);
            }

            try
            {
                string usuarioId;
                IdentityUser? usuario = null;

                var solicitudEnProgreso = await ObtenerSolicitudEnProgresoAsync();

                if (solicitudEnProgreso != null)
                {
                    usuarioId = solicitudEnProgreso.UsuarioID;
                    usuario = await _userManager.FindByIdAsync(usuarioId);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(modelo.DatosPersonales.Email))
                    {
                        ModelState.AddModelError(string.Empty, "El correo es obligatorio.");
                        modelo.PasoActual = 2;
                        return View("Paso2", modelo);
                    }

                    if (string.IsNullOrWhiteSpace(clave))
                    {
                        ModelState.AddModelError(string.Empty, "La contraseña es obligatoria.");
                        modelo.PasoActual = 2;
                        return View("Paso2", modelo);
                    }

                    usuario = await _userManager.FindByEmailAsync(modelo.DatosPersonales.Email);
                    if (usuario != null)
                    {
                        ModelState.AddModelError(string.Empty, "El correo ya está registrado.");
                        modelo.PasoActual = 2;
                        return View("Paso2", modelo);
                    }

                    usuario = new IdentityUser
                    {
                        UserName = modelo.DatosPersonales.Email,
                        Email = modelo.DatosPersonales.Email
                    };
                    var resultadoCreacion = await _userManager.CreateAsync(usuario, clave);
                    if (!resultadoCreacion.Succeeded)
                    {
                        foreach (var error in resultadoCreacion.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        modelo.PasoActual = 2;
                        return View("Paso2", modelo);
                    }

                    usuarioId = usuario.Id;
                }

                var usuario_db = new Usuario
                {
                    UsuarioID = usuarioId,
                    Nombre = modelo.DatosPersonales.Nombre,
                    Apellidos = modelo.DatosPersonales.Apellidos,
                    Email = modelo.DatosPersonales.Email,
                    Telefono = modelo.DatosPersonales.Telefono,
                    EstadoUsuario = "Inactivo"
                };

                SolicitudTecnico solicitud;

                if (solicitudEnProgreso == null)
                {
                    // El Usuario debe existir en la BD ANTES de crear el
                    // SolicitudTecnico que lo referencia por FK (UsuarioID).
                    _context.Add(usuario_db);
                    await _context.SaveChangesAsync();

                    solicitud = await CrearSolicitudBorradorAsync(usuarioId);
                }
                else
                {
                    solicitud = solicitudEnProgreso;
                    var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioID == usuarioId);
                    if (usuarioExistente != null)
                    {
                        usuarioExistente.Nombre = modelo.DatosPersonales.Nombre;
                        usuarioExistente.Apellidos = modelo.DatosPersonales.Apellidos;
                        usuarioExistente.Telefono = modelo.DatosPersonales.Telefono;
                        _context.Update(usuarioExistente);
                    }
                    else
                    {
                        _context.Add(usuario_db);
                    }
                }

                solicitud.Identificacion = modelo.DatosPersonales.Identificacion;
                solicitud.FechaUltimaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();
                HttpContext.Session.SetString(SesionSolicitudKey, solicitud.SolicitudTecnicoID.ToString());

                // Si el usuario aún no tenía sesión (era anónimo), la iniciamos
                // ahora que ya tiene cuenta creada. No le da rol Tecnico; eso
                // solo ocurre cuando la solicitud es aprobada.
                if (!(User.Identity?.IsAuthenticated ?? false))
                {
                    await _signInManager.SignInAsync(usuario!, isPersistent: false);
                }

                return RedirectToAction(nameof(Paso), new { paso = 3 });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al guardar: {ex.Message}" + (ex.InnerException != null ? $" | Detalle: {ex.InnerException.Message}" : ""));
                modelo.PasoActual = 2;
                return View("Paso2", modelo);
            }
        }

        // ---------------------------------------------------------------
        // Paso 3: Especialidades
        // ---------------------------------------------------------------
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarPaso3([Bind(Prefix = "especialidadesSeleccionadas")] List<EspecialidadSeleccionadaViewModel> especialidadesSeleccionadas)
        {
            try
            {
                if (especialidadesSeleccionadas == null || !especialidadesSeleccionadas.Any())
                {
                    ModelState.AddModelError(string.Empty, "Debe seleccionar al menos una especialidad.");
                    var solicitud = await ObtenerSolicitudEnProgresoAsync();
                    var vm = new SolicitudTecnicoWizardViewModel
                    {
                        PasoActual = 3,
                        Especialidades = new EspecialidadesStepViewModel { Categorias = await ObtenerCategoriasConServiciosAsync() }
                    };
                    if (solicitud != null)
                    {
                        vm.SolicitudTecnicoID = solicitud.SolicitudTecnicoID;
                        vm.CodigoSolicitud = solicitud.CodigoSolicitud;
                    }
                    return View("Paso3", vm);
                }

                var solicitudActual = await ObtenerSolicitudEnProgresoAsync();
                if (solicitudActual == null)
                {
                    ModelState.AddModelError(string.Empty, "No se encontró la solicitud en progreso.");
                    var vm2 = new SolicitudTecnicoWizardViewModel { PasoActual = 3 };
                    return View("Paso3", vm2);
                }

                var especialidadesAntiguas = _context.SolicitudTecnicoEspecialidades
                    .Where(e => e.SolicitudTecnicoID == solicitudActual.SolicitudTecnicoID)
                    .ToList();
                _context.SolicitudTecnicoEspecialidades.RemoveRange(especialidadesAntiguas);

                foreach (var esp in especialidadesSeleccionadas)
                {
                    var nueva = new SolicitudTecnicoEspecialidad
                    {
                        SolicitudTecnicoID = solicitudActual.SolicitudTecnicoID,
                        ServicioID = esp.ServicioID,
                        AniosExperiencia = esp.AniosExperiencia,
                        DescripcionExperiencia = esp.DescripcionExperiencia
                    };
                    _context.SolicitudTecnicoEspecialidades.Add(nueva);
                }

                solicitudActual.FechaUltimaActualizacion = DateTime.Now;
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString(SesionSolicitudKey, solicitudActual.SolicitudTecnicoID.ToString());
                return RedirectToAction(nameof(Paso), new { paso = 4 });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al guardar especialidades: {ex.Message}" + (ex.InnerException != null ? $" | Detalle: {ex.InnerException.Message}" : ""));
                var solicitud = await ObtenerSolicitudEnProgresoAsync();
                var vm = new SolicitudTecnicoWizardViewModel
                {
                    PasoActual = 3,
                    Especialidades = new EspecialidadesStepViewModel { Categorias = await ObtenerCategoriasConServiciosAsync() }
                };
                if (solicitud != null)
                {
                    vm.SolicitudTecnicoID = solicitud.SolicitudTecnicoID;
                    vm.CodigoSolicitud = solicitud.CodigoSolicitud;
                }
                return View("Paso3", vm);
            }
        }

        // ---------------------------------------------------------------
        // Paso 4: Movilidad
        // ---------------------------------------------------------------
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarPaso4(SolicitudTecnicoWizardViewModel modelo)
        {
            // Ignorar validaciones de otros pasos (ver comentario en GuardarPaso2).
            foreach (var key in ModelState.Keys.Where(k => !k.StartsWith("Movilidad")).ToList())
            {
                ModelState.Remove(key);
            }

            if (!ModelState.IsValid)
            {
                modelo.PasoActual = 4;
                return View("Paso4", modelo);
            }

            try
            {
                var solicitud = await ObtenerSolicitudEnProgresoAsync();
                if (solicitud == null)
                {
                    ModelState.AddModelError(string.Empty, "No se encontró la solicitud en progreso.");
                    modelo.PasoActual = 4;
                    return View("Paso4", modelo);
                }

                solicitud.TieneLicencia = modelo.Movilidad.TieneLicencia;
                solicitud.TipoLicencia = modelo.Movilidad.TipoLicencia;
                solicitud.TieneVehiculo = modelo.Movilidad.TieneVehiculo;
                solicitud.TipoVehiculo = modelo.Movilidad.TipoVehiculo;
                solicitud.ModalidadTrabajo = modelo.Movilidad.ModalidadTrabajo;
                solicitud.CantidadAyudantes = modelo.Movilidad.CantidadAyudantes;
                solicitud.EquipoHabitual = modelo.Movilidad.EquipoHabitual;
                solicitud.FechaUltimaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Paso), new { paso = 5 });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al guardar: {ex.Message}" + (ex.InnerException != null ? $" | Detalle: {ex.InnerException.Message}" : ""));
                modelo.PasoActual = 4;
                return View("Paso4", modelo);
            }
        }

        // ---------------------------------------------------------------
        // Paso 5: Cobertura
        // ---------------------------------------------------------------
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarPaso5(SolicitudTecnicoWizardViewModel modelo)
        {
            // Ignorar validaciones de otros pasos (ver comentario en GuardarPaso2).
            foreach (var key in ModelState.Keys.Where(k => !k.StartsWith("Cobertura")).ToList())
            {
                ModelState.Remove(key);
            }

            if (!ModelState.IsValid)
            {
                modelo.PasoActual = 5;
                ViewBag.Provincias = await _context.Provincias.Where(p => p.Activa).OrderBy(p => p.Nombre).ToListAsync();
                return View("Paso5", modelo);
            }

            try
            {
                var solicitud = await ObtenerSolicitudEnProgresoAsync();
                if (solicitud == null)
                {
                    ModelState.AddModelError(string.Empty, "No se encontró la solicitud en progreso.");
                    modelo.PasoActual = 5;
                    return View("Paso5", modelo);
                }

                var coberturasAntiguas = _context.SolicitudTecnicoCobertura
                    .Where(c => c.SolicitudTecnicoID == solicitud.SolicitudTecnicoID)
                    .ToList();
                _context.SolicitudTecnicoCobertura.RemoveRange(coberturasAntiguas);

                foreach (var zona in modelo.Cobertura.ZonasSeleccionadas)
                {
                    var nueva = new SolicitudTecnicoCobertura
                    {
                        SolicitudTecnicoID = solicitud.SolicitudTecnicoID,
                        ProvinciaID = zona.ProvinciaID,
                        CantonID = zona.CantonID,
                        DistritoID = zona.DistritoID
                    };
                    _context.SolicitudTecnicoCobertura.Add(nueva);
                }

                solicitud.FechaUltimaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Paso), new { paso = 6 });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al guardar: {ex.Message}" + (ex.InnerException != null ? $" | Detalle: {ex.InnerException.Message}" : ""));
                modelo.PasoActual = 5;
                return View("Paso5", modelo);
            }
        }

        // ---------------------------------------------------------------
        // Paso 6: Seguro y Accesibilidad
        // ---------------------------------------------------------------
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarPaso6(SolicitudTecnicoWizardViewModel modelo)
        {
            // Ignorar validaciones de otros pasos (ver comentario en GuardarPaso2).
            foreach (var key in ModelState.Keys.Where(k => !k.StartsWith("SeguroAccesibilidad")).ToList())
            {
                ModelState.Remove(key);
            }

            if (!ModelState.IsValid)
            {
                modelo.PasoActual = 6;
                return View("Paso6", modelo);
            }

            try
            {
                var solicitud = await ObtenerSolicitudEnProgresoAsync();
                if (solicitud == null)
                {
                    ModelState.AddModelError(string.Empty, "No se encontró la solicitud en progreso.");
                    modelo.PasoActual = 6;
                    return View("Paso6", modelo);
                }

                solicitud.TieneSeguro = modelo.SeguroAccesibilidad.TieneSeguro;
                solicitud.TipoSeguro = modelo.SeguroAccesibilidad.TipoSeguro;
                solicitud.NecesitaAccesibilidad = modelo.SeguroAccesibilidad.NecesitaAccesibilidad;
                solicitud.DetalleAccesibilidad = modelo.SeguroAccesibilidad.DetalleAccesibilidad;
                solicitud.FechaUltimaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Paso), new { paso = 7 });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al guardar: {ex.Message}" + (ex.InnerException != null ? $" | Detalle: {ex.InnerException.Message}" : ""));
                modelo.PasoActual = 6;
                return View("Paso6", modelo);
            }
        }

        // ---------------------------------------------------------------
        // Paso 7: Documentos
        // ---------------------------------------------------------------
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarPaso7(
            SolicitudTecnicoWizardViewModel modelo,
            List<int>? tiposDocumentoID,
            List<IFormFile>? archivos)
        {
            // Ignorar validaciones de otros pasos del wizard
            foreach (var key in ModelState.Keys.Where(k => !k.StartsWith("Documentos")).ToList())
            {
                ModelState.Remove(key);
            }

            try
            {
                var solicitud = await ObtenerSolicitudEnProgresoAsync();
                if (solicitud == null)
                {
                    ModelState.AddModelError(string.Empty, "No se encontró la solicitud en progreso.");
                    modelo.PasoActual = 7;
                    var tiposDoc = await _context.TiposDocumentoTecnico.Where(t => t.Activo).OrderBy(t => t.Nombre).ToListAsync();
                    ViewBag.TiposDocumento = tiposDoc;
                    return View("Paso7", modelo);
                }

                // Procesar archivos únicamente si el usuario seleccionó alguno
                if (archivos != null && archivos.Count > 0 && tiposDocumentoID != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documentos_tecnicos");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    for (int i = 0; i < archivos.Count; i++)
                    {
                        var archivo = archivos[i];
                        if (archivo != null && archivo.Length > 0 && i < tiposDocumentoID.Count)
                        {
                            int tipoId = tiposDocumentoID[i];

                            // Generar nombre único para evitar colisiones
                            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(archivo.FileName)}";
                            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await archivo.CopyToAsync(stream);
                            }

                            var rutaRelativa = $"/uploads/documentos_tecnicos/{uniqueFileName}";

                            var docExistente = await _context.SolicitudTecnicoDocumentos
                                .FirstOrDefaultAsync(d => d.SolicitudTecnicoID == solicitud.SolicitudTecnicoID
                                    && d.TipoDocumentoID == tipoId);

                            if (docExistente == null)
                            {
                                docExistente = new SolicitudTecnicoDocumento
                                {
                                    SolicitudTecnicoID = solicitud.SolicitudTecnicoID,
                                    TipoDocumentoID = tipoId,
                                    EstadoDocumento = "PENDIENTE"
                                };
                                _context.SolicitudTecnicoDocumentos.Add(docExistente);
                            }

                            docExistente.NombreArchivo = archivo.FileName;
                            docExistente.RutaArchivo = rutaRelativa;
                            docExistente.FechaCarga = DateTime.Now;
                        }
                    }
                }

                // Actualizar la fecha de última modificación de la solicitud
                solicitud.FechaUltimaActualizacion = DateTime.Now;
                await _context.SaveChangesAsync();

                // Avanzar directamente al Paso 8 sin bloquear si no hay archivos nuevos
                return RedirectToAction(nameof(Paso), new { paso = 8 });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al guardar: {ex.Message}" + (ex.InnerException != null ? $" | Detalle: {ex.InnerException.Message}" : ""));
                modelo.PasoActual = 7;
                var tiposDocumento = await _context.TiposDocumentoTecnico.Where(t => t.Activo).OrderBy(t => t.Nombre).ToListAsync();
                ViewBag.TiposDocumento = tiposDocumento;
                return View("Paso7", modelo);
            }
        }

        // ---------------------------------------------------------------
        // Paso 8: Revisión y Envío
        // ---------------------------------------------------------------
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarPaso8(SolicitudTecnicoWizardViewModel modelo)
        {
            try
            {
                var solicitudActual = await ObtenerSolicitudEnProgresoAsync();
                if (solicitudActual == null)
                {
                    ModelState.AddModelError(string.Empty, "No se encontró la solicitud en progreso.");
                    return View("Paso8", new SolicitudTecnicoWizardViewModel { PasoActual = 8 });
                }

                // Validar confirmación usando el modelo recibido
                if (!modelo.ConfirmaInformacionVeridica)
                {
                    ModelState.AddModelError(string.Empty, "Debe confirmar que la información es verdadera.");
                    modelo.PasoActual = 8;
                    modelo.CodigoSolicitud = solicitudActual.CodigoSolicitud;
                    await CargarModeloDesdeSolicitudAsync(modelo, solicitudActual);
                    return View("Paso8", modelo);
                }

                // Cambiar estado a ENVIADA
                var estadoEnviada = await _context.EstadosSolicitudTecnico
                    .FirstOrDefaultAsync(e => e.Codigo == "ENVIADA");

                if (estadoEnviada == null)
                {
                    ModelState.AddModelError(string.Empty, "No se puede cambiar el estado de la solicitud. Contacte al administrador.");
                    modelo.PasoActual = 8;
                    modelo.CodigoSolicitud = solicitudActual.CodigoSolicitud;
                    await CargarModeloDesdeSolicitudAsync(modelo, solicitudActual);
                    return View("Paso8", modelo);
                }

                solicitudActual.EstadoSolicitudTecnicoID = estadoEnviada.EstadoSolicitudTecnicoID;
                solicitudActual.FechaEnvio = DateTime.Now;
                solicitudActual.FechaUltimaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();

                // Limpiar sesión - solicitud completada
                HttpContext.Session.Remove(SesionSolicitudKey);

                // Retornar a la vista de éxito pasando el modelo o la solicitud para mostrar el código
                return View("Index", solicitudActual); // O redirigir a una acción de éxito si lo prefieres
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al guardar: {ex.Message}" + (ex.InnerException != null ? $" | Detalle: {ex.InnerException.Message}" : ""));
                var vmError = new SolicitudTecnicoWizardViewModel { PasoActual = 8 };
                return View("Paso8", vmError);
            }
        }

        // ---------------------------------------------------------------
        // Métodos auxiliares privados
        // ---------------------------------------------------------------

        /// <summary>
        /// Obtiene la solicitud en progreso desde la sesión.
        /// </summary>
        private async Task<SolicitudTecnico?> ObtenerSolicitudEnProgresoAsync()
        {
            var solicitudIdStr = HttpContext.Session.GetString(SesionSolicitudKey);
            if (string.IsNullOrEmpty(solicitudIdStr) || !long.TryParse(solicitudIdStr, out var solicitudId))
                return null;

            return await _context.SolicitudesTecnico
                .AsSplitQuery()
                .Include(s => s.Usuario)
                .Include(s => s.EstadoSolicitud)
                .Include(s => s.Especialidades).ThenInclude(e => e.Servicio)
                .Include(s => s.Cobertura).ThenInclude(c => c.Provincia)
                .Include(s => s.Cobertura).ThenInclude(c => c.Canton)
                .Include(s => s.Cobertura).ThenInclude(c => c.Distrito)
                .Include(s => s.Documentos).ThenInclude(d => d.TipoDocumento)
                .FirstOrDefaultAsync(s => s.SolicitudTecnicoID == solicitudId);
        }

        /// <summary>
        /// Crea una solicitud en estado BORRADOR.
        /// </summary>
        private async Task<SolicitudTecnico> CrearSolicitudBorradorAsync(string usuarioId)
        {
            var estadoBorrador = await _context.EstadosSolicitudTecnico
                .FirstOrDefaultAsync(e => e.Codigo == "BORRADOR");

            if (estadoBorrador == null)
            {
                throw new InvalidOperationException("No se encontró el estado 'BORRADOR' en la base de datos.");
            }

            var solicitud = new SolicitudTecnico
            {
                UsuarioID = usuarioId,
                EstadoSolicitudTecnicoID = estadoBorrador.EstadoSolicitudTecnicoID,
                CodigoSolicitud = GenerarCodigoSolicitud(),
                FechaCreacion = DateTime.Now,
                FechaUltimaActualizacion = DateTime.Now
            };

            _context.SolicitudesTecnico.Add(solicitud);
            await _context.SaveChangesAsync();

            return solicitud;
        }

        /// <summary>
        /// Carga los datos de la solicitud en el ViewModel.
        /// </summary>
        private async Task CargarModeloDesdeSolicitudAsync(SolicitudTecnicoWizardViewModel modelo, SolicitudTecnico solicitud)
        {
            // Paso 2: Datos Personales
            if (solicitud.Usuario != null)
            {
                modelo.DatosPersonales.Nombre = solicitud.Usuario.Nombre;
                modelo.DatosPersonales.Apellidos = solicitud.Usuario.Apellidos;
                modelo.DatosPersonales.Email = solicitud.Usuario.Email;
                modelo.DatosPersonales.Telefono = solicitud.Usuario.Telefono;
                modelo.DatosPersonales.Identificacion = solicitud.Identificacion;
            }

            // Paso 3: Especialidades
            if (solicitud.Especialidades != null && solicitud.Especialidades.Any())
            {
                modelo.Especialidades.EspecialidadesSeleccionadas = solicitud.Especialidades
                    .Select(e => new EspecialidadSeleccionadaViewModel
                    {
                        ServicioID = e.ServicioID,
                        NombreServicio = e.Servicio?.NombreServicio,
                        AniosExperiencia = e.AniosExperiencia,
                        DescripcionExperiencia = e.DescripcionExperiencia
                    }).ToList();
            }

            // Paso 4: Movilidad
            modelo.Movilidad.TieneLicencia = solicitud.TieneLicencia;
            modelo.Movilidad.TipoLicencia = solicitud.TipoLicencia;
            modelo.Movilidad.TieneVehiculo = solicitud.TieneVehiculo;
            modelo.Movilidad.TipoVehiculo = solicitud.TipoVehiculo;
            modelo.Movilidad.ModalidadTrabajo = solicitud.ModalidadTrabajo ?? "SOLO";
            modelo.Movilidad.CantidadAyudantes = solicitud.CantidadAyudantes;
            modelo.Movilidad.EquipoHabitual = solicitud.EquipoHabitual;

            // Paso 5: Cobertura
            if (solicitud.Cobertura != null && solicitud.Cobertura.Any())
            {
                modelo.Cobertura.ZonasSeleccionadas = solicitud.Cobertura
                    .Select(c => new CoberturaSeleccionadaViewModel
                    {
                        ProvinciaID = c.ProvinciaID,
                        CantonID = c.CantonID,
                        DistritoID = c.DistritoID,
                        NombreProvincia = c.Provincia?.Nombre,
                        NombreCanton = c.Canton?.Nombre,
                        NombreDistrito = c.Distrito?.Nombre
                    }).ToList();
            }
            modelo.Cobertura.RadioCoberturaKm = 20;

            // Paso 6: Seguro y Accesibilidad
            modelo.SeguroAccesibilidad.TieneSeguro = solicitud.TieneSeguro;
            modelo.SeguroAccesibilidad.TipoSeguro = solicitud.TipoSeguro;
            modelo.SeguroAccesibilidad.NecesitaAccesibilidad = solicitud.NecesitaAccesibilidad;
            modelo.SeguroAccesibilidad.DetalleAccesibilidad = solicitud.DetalleAccesibilidad;

            // Paso 7: Documentos
            if (solicitud.Documentos != null && solicitud.Documentos.Any())
            {
                modelo.Documentos.Documentos = solicitud.Documentos
                    .Select(d => new DocumentoCargaViewModel
                    {
                        TipoDocumentoID = d.TipoDocumentoID,
                        NombreTipoDocumento = d.TipoDocumento?.Nombre,
                        NombreArchivo = d.NombreArchivo,
                        RutaArchivo = d.RutaArchivo,
                        Obligatorio = d.TipoDocumento?.Obligatorio ?? false
                    }).ToList();
            }

            // Paso 8: Confirmación
            modelo.ConfirmaInformacionVeridica = false; // Siempre false al cargar
        }

        /// <summary>
        /// Obtiene el paso actual según el estado de la solicitud.
        /// </summary>
        private int ObtenerPasoSegunEstado(SolicitudTecnico solicitud)
        {
            return solicitud.EstadoSolicitud?.Codigo switch
            {
                "BORRADOR" => 2,
                "ENVIADA" => 8,
                "EN_REVISION" => 8,
                "RECHAZADA" => 2,
                _ => 2
            };
        }

        /// <summary>
        /// Genera un código único para la solicitud.
        /// </summary>
        private string GenerarCodigoSolicitud()
        {
            return $"SOL-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        /// <summary>
        /// Obtiene las categorías y servicios disponibles.
        /// </summary>
        private async Task<List<CategoriaSeleccionViewModel>> ObtenerCategoriasConServiciosAsync()
        {
            var categorias = await _context.CategoriasServicio
                .Where(c => c.Activa)
                .OrderBy(c => c.NombreCategoria)
                .Select(c => new CategoriaSeleccionViewModel
                {
                    CategoriaID = c.CategoriaID,
                    NombreCategoria = c.NombreCategoria,
                    Servicios = c.Servicios
                        .Where(s => s.Activo)
                        .OrderBy(s => s.NombreServicio)
                        .Select(s => new ServicioSeleccionViewModel
                        {
                            ServicioID = s.ServicioID,
                            NombreServicio = s.NombreServicio
                        }).ToList()
                }).ToListAsync();

            return categorias;
        }
    }
}