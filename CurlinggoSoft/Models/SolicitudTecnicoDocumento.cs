using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    // Documento subido por el aspirante durante el paso 7 del wizard
    // (identificación, licencia, certificación, seguro, etc.).
    [Table("SolicitudTecnicoDocumentos")]
    public class SolicitudTecnicoDocumento
    {
        [Key]
        [Display(Name = "ID Documento")]
        public long SolicitudTecnicoDocumentoID { get; set; }

        [Required]
        [Display(Name = "Solicitud")]
        public long SolicitudTecnicoID { get; set; }

        [Required]
        [Display(Name = "Tipo de Documento")]
        public int TipoDocumentoID { get; set; }

        [Required(ErrorMessage = "El nombre del archivo es obligatorio")]
        [StringLength(255)]
        [Display(Name = "Nombre de Archivo")]
        public string NombreArchivo { get; set; } = null!;

        [Required(ErrorMessage = "La ruta del archivo es obligatoria")]
        [StringLength(500)]
        [Display(Name = "Ruta de Archivo")]
        public string RutaArchivo { get; set; } = null!;

        [Display(Name = "Fecha de Carga")]
        public DateTime FechaCarga { get; set; } = DateTime.Now;

        [Required]
        [StringLength(30)]
        [Display(Name = "Estado del Documento")]
        public string EstadoDocumento { get; set; } = "PENDIENTE"; // PENDIENTE / APROBADO / RECHAZADO

        [StringLength(450)]
        [Display(Name = "Revisado Por")]
        public string? RevisadoPor { get; set; }

        [Display(Name = "Fecha de Revisión")]
        public DateTime? FechaRevision { get; set; }

        [StringLength(1000)]
        public string? Observaciones { get; set; }

        [ForeignKey("SolicitudTecnicoID")]
        public virtual SolicitudTecnico? Solicitud { get; set; }

        [ForeignKey("TipoDocumentoID")]
        public virtual TipoDocumentoTecnico? TipoDocumento { get; set; }
    }
}
