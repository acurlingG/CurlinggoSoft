using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("Permisos")]
    public class Permiso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Permiso")]
        public int PermisoID { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Código Permiso")]
        public string CodigoPermiso { get; set; } = null!;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150)]
        public string Nombre { get; set; } = null!;

        [StringLength(300)]
        public string? Descripcion { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
    }
}
