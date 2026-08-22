using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("ClientesPerfil")]
    public class ClientePerfil
    {
        [Key]
        [StringLength(450)]
        [Display(Name = "ID Cliente")]
        public string ClienteID { get; set; } = null!;

        [Required(ErrorMessage = "La provincia es obligatoria")]
        [Display(Name = "Provincia")]
        public int ProvinciaID { get; set; }

        [Required(ErrorMessage = "El cantón es obligatorio")]
        [Display(Name = "Cantón")]
        public int CantonID { get; set; }

        [Required(ErrorMessage = "El distrito es obligatorio")]
        [Display(Name = "Distrito")]
        public int DistritoID { get; set; }

        [Required(ErrorMessage = "La dirección exacta es obligatoria")]
        [StringLength(300)]
        [Display(Name = "Dirección Exacta")]
        public string DireccionExacta { get; set; } = null!;

        [Column(TypeName = "decimal(9,6)")]
        public decimal? Latitud { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal? Longitud { get; set; }

        [Display(Name = "Fecha Actualización")]
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(3,2)")]
        [Range(0.00, 5.00, ErrorMessage = "La calificación promedio debe estar entre 0.00 y 5.00")]
        [Display(Name = "Calificación Promedio")]
        public decimal CalificacionPromedio { get; set; } = 0.00m;

        [ForeignKey("ProvinciaID")]
        public virtual Provincia? Provincia { get; set; }

        [ForeignKey("CantonID")]
        public virtual Canton? Canton { get; set; }

        [ForeignKey("DistritoID")]
        public virtual Distrito? Distrito { get; set; }
    }
}
