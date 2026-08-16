using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("Cantones")]
    public class Canton
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ID Cantón")]
        public int CantonID { get; set; }

        [Required(ErrorMessage = "La provincia es obligatoria")]
        [Display(Name = "Provincia")]
        public int ProvinciaID { get; set; }

        [Required(ErrorMessage = "El nombre del cantón es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El código DTA es obligatorio")]
        [StringLength(3)]
        [Display(Name = "Código DTA")]
        public string CodigoDTA { get; set; } = null!;

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        // Relación
        [ForeignKey("ProvinciaID")]
        public virtual Provincia Provincia { get; set; } = null!;
    }
}
