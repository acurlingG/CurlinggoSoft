using System.ComponentModel.DataAnnotations;

namespace CurlinggoSoft.Models.ViewModels
{
    // ViewModel único que agrupa TODOS los campos de los 8 pasos del wizard
    // de "Registro de Técnico". Se persiste entre pasos usando TempData/Session
    // o guardando incrementalmente en SolicitudTecnico (recomendado: guardar
    // en BD en cada "Siguiente" para soportar "Guardar y salir").
    //
    // Mapeo general:
    //   Paso 1 -> sin datos (solo consentimiento/bienvenida)
    //   Paso 2 -> DatosPersonales      -> Usuarios + SolicitudesTecnico.Identificacion
    //   Paso 3 -> Especialidades       -> SolicitudTecnicoEspecialidades
    //   Paso 4 -> Movilidad            -> SolicitudesTecnico (campos de movilidad/equipo)
    //   Paso 5 -> Cobertura            -> SolicitudTecnicoCobertura
    //   Paso 6 -> SeguroAccesibilidad  -> SolicitudesTecnico (seguro/accesibilidad)
    //   Paso 7 -> Documentos           -> SolicitudTecnicoDocumentos + BackgroundCheck
    //   Paso 8 -> Revisión y envío     -> sin campos propios, solo confirmación
    public class SolicitudTecnicoWizardViewModel
    {
        // Identifica la solicitud en progreso (null hasta que se crea el
        // registro en estado BORRADOR, en el Paso 1 -> "Comenzar").
        public long? SolicitudTecnicoID { get; set; }

        public string? CodigoSolicitud { get; set; }

        // Paso actual del wizard (1-8), usado para renderizar la barra de
        // progreso y validar solo la sección correspondiente.
        [Range(1, 8)]
        public int PasoActual { get; set; } = 1;

        public DatosPersonalesStepViewModel DatosPersonales { get; set; } = new();

        public EspecialidadesStepViewModel Especialidades { get; set; } = new();

        public MovilidadStepViewModel Movilidad { get; set; } = new();

        public CoberturaStepViewModel Cobertura { get; set; } = new();

        public SeguroAccesibilidadStepViewModel SeguroAccesibilidad { get; set; } = new();

        public DocumentosStepViewModel Documentos { get; set; } = new();

        // Paso 8: solo requiere la confirmación final.
        [Range(typeof(bool), "true", "true", ErrorMessage = "Debe confirmar que la información es verdadera.")]
        public bool ConfirmaInformacionVeridica { get; set; }
    }

    // --- Paso 2: Datos personales ---
    public class DatosPersonalesStepViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100)]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "La identificación es obligatoria")]
        [StringLength(30)]
        [Display(Name = "Identificación")]
        public string Identificacion { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        [StringLength(150)]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(30)]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = null!;

        [StringLength(30)]
        [Display(Name = "Teléfono Secundario")]
        public string? TelefonoSecundario { get; set; }
    }

    // --- Paso 3: Especialidades + experiencia ---
    // Estructura jerárquica para el UI: Categoría -> Servicios (checkbox) ->
    // si se marca, capturar AniosExperiencia + DescripcionExperiencia.
    public class EspecialidadesStepViewModel
    {
        public List<CategoriaSeleccionViewModel> Categorias { get; set; } = new();

        // Lista plana de especialidades seleccionadas, la que realmente se
        // guarda en SolicitudTecnicoEspecialidades al avanzar de paso.
        public List<EspecialidadSeleccionadaViewModel> EspecialidadesSeleccionadas { get; set; } = new();
    }

    public class CategoriaSeleccionViewModel
    {
        public int CategoriaID { get; set; }
        public string NombreCategoria { get; set; } = null!;
        public List<ServicioSeleccionViewModel> Servicios { get; set; } = new();
    }

    public class ServicioSeleccionViewModel
    {
        public int ServicioID { get; set; }
        public string NombreServicio { get; set; } = null!;
        public bool Seleccionado { get; set; }
    }

    public class EspecialidadSeleccionadaViewModel
    {
        [Required]
        public int ServicioID { get; set; }

        public string? NombreServicio { get; set; }

        [Range(0, 60, ErrorMessage = "Los años de experiencia deben estar entre 0 y 60")]
        [Display(Name = "Años de Experiencia")]
        public int AniosExperiencia { get; set; }

        [StringLength(1000)]
        [Display(Name = "Descripción de la Experiencia")]
        public string? DescripcionExperiencia { get; set; }
    }

    // --- Paso 4: Movilidad y equipo de trabajo ---
    public class MovilidadStepViewModel
    {
        [Display(Name = "¿Tiene licencia de conducir?")]
        public bool? TieneLicencia { get; set; }

        // MOTO / AUTOMOVIL / AMBAS / OTRA (solo aplica si TieneLicencia = true)
        [Display(Name = "Tipo de Licencia")]
        public string? TipoLicencia { get; set; }

        [Display(Name = "¿Cuenta con vehículo para trabajar?")]
        public bool? TieneVehiculo { get; set; }

        // MOTOCICLETA / AUTOMOVIL / CAMIONETA / OTRO (solo si TieneVehiculo = true)
        [Display(Name = "Tipo de Vehículo")]
        public string? TipoVehiculo { get; set; }

        // SOLO / UN_AYUDANTE / DOS_O_MAS
        [Required(ErrorMessage = "Indique cómo realiza normalmente sus trabajos")]
        [Display(Name = "Modalidad de Trabajo")]
        public string ModalidadTrabajo { get; set; } = "SOLO";

        // Solo aplica si ModalidadTrabajo == DOS_O_MAS (2-50); si es
        // UN_AYUDANTE se fuerza a 1 en el controller.
        [Range(1, 50)]
        [Display(Name = "Cantidad de Ayudantes")]
        public int? CantidadAyudantes { get; set; }

        [Display(Name = "¿Forman parte de su equipo habitual?")]
        public bool? EquipoHabitual { get; set; }
    }

    // --- Paso 5: Zona de cobertura ---
    public class CoberturaStepViewModel
    {
        public List<CoberturaSeleccionadaViewModel> ZonasSeleccionadas { get; set; } = new();

        [Required(ErrorMessage = "Indique hasta qué distancia está dispuesto a desplazarse")]
        [Range(1, 200, ErrorMessage = "El radio debe estar entre 1 y 200 km")]
        [Display(Name = "Radio de Cobertura (km)")]
        public decimal RadioCoberturaKm { get; set; } = 20;
    }

    public class CoberturaSeleccionadaViewModel
    {
        [Required]
        public int ProvinciaID { get; set; }

        [Required]
        public int CantonID { get; set; }

        public int? DistritoID { get; set; }

        public string? NombreProvincia { get; set; }
        public string? NombreCanton { get; set; }
        public string? NombreDistrito { get; set; }
    }

    // --- Paso 6: Seguro y condiciones personales ---
    public class SeguroAccesibilidadStepViewModel
    {
        [Display(Name = "¿Cuenta actualmente con seguro?")]
        public bool? TieneSeguro { get; set; }

        // RIESGOS_TRABAJO / SEGURO_VOLUNTARIO / SEGURO_PRIVADO / OTRO
        [Display(Name = "Tipo de Seguro")]
        public string? TipoSeguro { get; set; }

        // NULL = no respondió; se traduce a una opción tri-estado en la UI
        // (No / Sí / Prefiero no responder).
        [Display(Name = "¿Necesita alguna consideración de accesibilidad?")]
        public bool? NecesitaAccesibilidad { get; set; }

        [StringLength(1000)]
        [Display(Name = "Detalle de Accesibilidad")]
        public string? DetalleAccesibilidad { get; set; }
    }

    // --- Paso 7: Documentos + Background Check ---
    public class DocumentosStepViewModel
    {
        // Un documento pendiente/cargado por cada TipoDocumentoTecnico activo.
        public List<DocumentoCargaViewModel> Documentos { get; set; } = new();

        [Range(typeof(bool), "true", "true", ErrorMessage = "Debe autorizar la verificación de antecedentes.")]
        [Display(Name = "Autorizo la verificación de antecedentes")]
        public bool AutorizaBackgroundCheck { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Debe aceptar los términos y condiciones.")]
        [Display(Name = "Acepto los términos y condiciones")]
        public bool AceptaTerminos { get; set; }
    }

    public class DocumentoCargaViewModel
    {
        public int TipoDocumentoID { get; set; }
        public string NombreTipoDocumento { get; set; } = null!;
        public bool Obligatorio { get; set; }

        // Nombre/ruta del archivo ya cargado (si existe), para mostrar el
        // estado "✓ Subido" en el paso de revisión.
        public string? NombreArchivo { get; set; }
        public string? RutaArchivo { get; set; }
    }
}
