using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("TecnicosPerfil")]
    public class TecnicoPerfil
    {
        [Key]
        [StringLength(450)]
        [Display(Name = "ID Técnico")]
        public string TecnicoID { get; set; } = null!;

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [StringLength(30)]
        [Display(Name = "Cédula")]
        public string IdentificacionCedula { get; set; } = null!;

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado Verificación")]
        public string EstadoVerificacion { get; set; } = "Pendiente";

        [Column(TypeName = "decimal(3,2)")]
        [Display(Name = "Calificación Promedio")]
        public decimal CalificacionPromedio { get; set; } = 0.00m;

        [Display(Name = "Disponible")]
        public bool Disponible { get; set; } = true;

        [Display(Name = "Provincia Cobertura")]
        public int? ProvinciaCoberturaID { get; set; }

        [Display(Name = "Cantón Cobertura")]
        public int? CantonCoberturaID { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal? LatitudActual { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal? LongitudActual { get; set; }

        [Display(Name = "Fecha Verificación")]
        public DateTime? FechaVerificacion { get; set; }

        [ForeignKey("ProvinciaCoberturaID")]
        public virtual Provincia? ProvinciaCobertura { get; set; }

        [ForeignKey("CantonCoberturaID")]
        public virtual Canton? CantonCobertura { get; set; }
    }
}
