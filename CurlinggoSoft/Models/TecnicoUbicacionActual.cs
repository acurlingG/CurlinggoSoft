using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("TecnicosUbicacionActual")]
    public class TecnicoUbicacionActual
    {
        [Key]
        [StringLength(450)]
        [Display(Name = "ID Técnico")]
        public string TecnicoID { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(9,6)")]
        [Display(Name = "Latitud")]
        public decimal Latitud { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,6)")]
        [Display(Name = "Longitud")]
        public decimal Longitud { get; set; }

        [Display(Name = "Última Actualización")]
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;

        [ForeignKey("TecnicoID")]
        public virtual TecnicoPerfil? Tecnico { get; set; }
    }
}
