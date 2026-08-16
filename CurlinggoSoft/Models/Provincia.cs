using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("Provincias")]
    public class Provincia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // La llave primaria no es autonumérica
        [Display(Name = "ID Provincia")]
        public int ProvinciaID { get; set; }

        [Required(ErrorMessage = "El nombre de la provincia es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El código DTA es obligatorio")]
        [StringLength(1)]
        [Display(Name = "Código DTA")]
        public string CodigoDTA { get; set; } = null!;

        public bool Activa { get; set; } = true;
    }
}