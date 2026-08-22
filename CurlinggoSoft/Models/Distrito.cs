using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CurlinggoSoft.Models
{
    [Table("Distritos")]
    public class Distrito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ID Distrito")]
        public int DistritoID { get; set; }

        [Required(ErrorMessage = "El cantón es obligatorio")]
        [Display(Name = "Cantón")]
        public int CantonID { get; set; }

        [Required(ErrorMessage = "El nombre del distrito es obligatorio")]
        [StringLength(150)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El código DTA es obligatorio")]
        [StringLength(5)]
        [Display(Name = "Código DTA")]
        public string CodigoDTA { get; set; } = null!;

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        // Relación
        [ForeignKey("CantonID")]
        [ValidateNever]
        public virtual Canton Canton { get; set; } = null!;
    }
}
