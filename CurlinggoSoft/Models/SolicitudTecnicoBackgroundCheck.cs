using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    // Estado de la verificación de antecedentes (background check) de una
    // solicitud de técnico. Relación 1:1 con SolicitudTecnico.
    [Table("SolicitudTecnicoBackgroundCheck")]
    public class SolicitudTecnicoBackgroundCheck
    {
        [Key]
        [Display(Name = "ID Background Check")]
        public long BackgroundCheckID { get; set; }

        [Required]
        [Display(Name = "Solicitud")]
        public long SolicitudTecnicoID { get; set; }

        [Required]
        [StringLength(30)]
        // PENDIENTE / AUTORIZADO / EN_PROCESO / COMPLETADO / APROBADO / RECHAZADO / REQUIERE_REVISION
        public string Estado { get; set; } = "PENDIENTE";

        [Display(Name = "Fecha de Autorización")]
        public DateTime? FechaAutorizacion { get; set; }

        [Display(Name = "Fecha de Inicio")]
        public DateTime? FechaInicio { get; set; }

        [Display(Name = "Fecha de Finalización")]
        public DateTime? FechaFinalizacion { get; set; }

        [StringLength(30)]
        // APROBADO / RECHAZADO / REQUIERE_REVISION
        public string? Resultado { get; set; }

        [StringLength(450)]
        [Display(Name = "Revisado Por")]
        public string? RevisadoPor { get; set; }

        [Display(Name = "Fecha de Revisión")]
        public DateTime? FechaRevision { get; set; }

        [StringLength(2000)]
        public string? Observaciones { get; set; }

        [ForeignKey("SolicitudTecnicoID")]
        public virtual SolicitudTecnico? Solicitud { get; set; }
    }
}
