using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    // Zona(s) de cobertura declaradas por el aspirante (paso 5 del wizard).
    // A diferencia de TecnicosPerfil (que solo admite una cobertura principal),
    // aquí se permiten múltiples cantones/distritos por solicitud.
    [Table("SolicitudTecnicoCobertura")]
    public class SolicitudTecnicoCobertura
    {
        [Key]
        [Display(Name = "ID Cobertura")]
        public long SolicitudTecnicoCoberturaID { get; set; }

        [Required]
        [Display(Name = "Solicitud")]
        public long SolicitudTecnicoID { get; set; }

        [Required(ErrorMessage = "La provincia es obligatoria")]
        [Display(Name = "Provincia")]
        public int ProvinciaID { get; set; }

        [Required(ErrorMessage = "El cantón es obligatorio")]
        [Display(Name = "Cantón")]
        public int CantonID { get; set; }

        [Display(Name = "Distrito")]
        public int? DistritoID { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "Radio de Cobertura (km)")]
        public decimal? RadioCoberturaKm { get; set; }

        [ForeignKey("SolicitudTecnicoID")]
        public virtual SolicitudTecnico? Solicitud { get; set; }

        [ForeignKey("ProvinciaID")]
        public virtual Provincia? Provincia { get; set; }

        [ForeignKey("CantonID")]
        public virtual Canton? Canton { get; set; }

        [ForeignKey("DistritoID")]
        public virtual Distrito? Distrito { get; set; }
    }
}
