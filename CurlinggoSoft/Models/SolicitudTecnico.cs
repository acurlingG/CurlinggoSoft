using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    // Expediente principal del aspirante a técnico (wizard de 8 pasos).
    // Representa el proceso de evaluación; SOLO cuando es aprobada (vía
    // usp_SolicitudTecnico_Aprobar) se crea/actualiza el TecnicoPerfil real.
    [Table("SolicitudesTecnico")]
    public class SolicitudTecnico
    {
        [Key]
        [Display(Name = "ID Solicitud")]
        public long SolicitudTecnicoID { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(450)]
        [Display(Name = "Usuario")]
        public string UsuarioID { get; set; } = null!;

        [Required]
        [StringLength(30)]
        [Display(Name = "Código de Solicitud")]
        public string CodigoSolicitud { get; set; } = null!;

        [Required]
        [Display(Name = "Estado")]
        public int EstadoSolicitudTecnicoID { get; set; }

        [StringLength(30)]
        [Display(Name = "Identificación")]
        public string? Identificacion { get; set; }

        // --- Movilidad ---
        [Display(Name = "¿Tiene licencia?")]
        public bool? TieneLicencia { get; set; }

        [StringLength(30)]
        [Display(Name = "Tipo de Licencia")]
        public string? TipoLicencia { get; set; } // MOTO / AUTOMOVIL / AMBAS / OTRA

        [Display(Name = "¿Tiene vehículo?")]
        public bool? TieneVehiculo { get; set; }

        [StringLength(30)]
        [Display(Name = "Tipo de Vehículo")]
        public string? TipoVehiculo { get; set; } // MOTOCICLETA / AUTOMOVIL / CAMIONETA / OTRO

        // --- Equipo de trabajo ---
        [StringLength(30)]
        [Display(Name = "Modalidad de Trabajo")]
        public string? ModalidadTrabajo { get; set; } // SOLO / UN_AYUDANTE / DOS_O_MAS

        [Range(1, 50)]
        [Display(Name = "Cantidad de Ayudantes")]
        public int? CantidadAyudantes { get; set; }

        [Display(Name = "¿Equipo Habitual?")]
        public bool? EquipoHabitual { get; set; }

        // --- Seguro ---
        [Display(Name = "¿Tiene Seguro?")]
        public bool? TieneSeguro { get; set; }

        [StringLength(50)]
        [Display(Name = "Tipo de Seguro")]
        public string? TipoSeguro { get; set; } // RIESGOS_TRABAJO / SEGURO_VOLUNTARIO / SEGURO_PRIVADO / OTRO

        // --- Accesibilidad ---
        [Display(Name = "¿Necesita Accesibilidad?")]
        public bool? NecesitaAccesibilidad { get; set; }

        [StringLength(1000)]
        [Display(Name = "Detalle de Accesibilidad")]
        public string? DetalleAccesibilidad { get; set; }

        // --- Auditoría del expediente ---
        [Display(Name = "Fecha Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Display(Name = "Fecha Última Actualización")]
        public DateTime? FechaUltimaActualizacion { get; set; }

        [Display(Name = "Fecha Envío")]
        public DateTime? FechaEnvio { get; set; }

        [Display(Name = "Fecha Revisión")]
        public DateTime? FechaRevision { get; set; }

        [StringLength(450)]
        [Display(Name = "Revisado Por")]
        public string? RevisadoPor { get; set; }

        [Display(Name = "Fecha Decisión")]
        public DateTime? FechaDecision { get; set; }

        [StringLength(1000)]
        [Display(Name = "Motivo de Rechazo")]
        public string? MotivoRechazo { get; set; }

        [StringLength(2000)]
        [Display(Name = "Observaciones Admin")]
        public string? ObservacionesAdmin { get; set; }

        [ForeignKey("UsuarioID")]
        public virtual Usuario? Usuario { get; set; }

        [ForeignKey("EstadoSolicitudTecnicoID")]
        public virtual EstadoSolicitudTecnico? EstadoSolicitud { get; set; }

        public virtual ICollection<SolicitudTecnicoEspecialidad> Especialidades { get; set; } = new List<SolicitudTecnicoEspecialidad>();
        public virtual ICollection<SolicitudTecnicoCobertura> Cobertura { get; set; } = new List<SolicitudTecnicoCobertura>();
        public virtual ICollection<SolicitudTecnicoDocumento> Documentos { get; set; } = new List<SolicitudTecnicoDocumento>();
        public virtual SolicitudTecnicoBackgroundCheck? BackgroundCheck { get; set; }
    }
}