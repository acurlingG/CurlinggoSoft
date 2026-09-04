using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    // Zonas de cobertura del técnico activo (permite múltiples áreas de servicio)
    [Table("TecnicoCoberturas")]
    public class TecnicoCobertura
    {
        [Key]
        [Display(Name = "ID Cobertura")]
        public long TecnicoCoberturaID { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Técnico")]
        public string TecnicoID { get; set; } = null!;

        [Required(ErrorMessage = "La provincia es obligatoria")]
        [Display(Name = "Provincia")]
        public int ProvinciaID { get; set; }

        [Required(ErrorMessage = "El cantón es obligatorio")]
        [Display(Name = "Cantón")]
        public int CantonID { get; set; }

        [Display(Name = "Distrito (Opcional)")]
        public int? DistritoID { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "Radio de Cobertura (km)")]
        public decimal? RadioCoberturaKm { get; set; }

        [Display(Name = "Activa")]
        public bool Activa { get; set; } = true;

        [Display(Name = "Fecha Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [ForeignKey("TecnicoID")]
        public virtual TecnicoPerfil? Tecnico { get; set; }

        [ForeignKey("ProvinciaID")]
        public virtual Provincia? Provincia { get; set; }

        [ForeignKey("CantonID")]
        public virtual Canton? Canton { get; set; }

        [ForeignKey("DistritoID")]
        public virtual Distrito? Distrito { get; set; }
    }
}
