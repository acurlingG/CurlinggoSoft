using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("Menus")]
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Menú")]
        public long MenuID { get; set; }

        [Display(Name = "Menú Padre")]
        public long? MenuPadreID { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [StringLength(300)]
        public string? Url { get; set; }

        [StringLength(100)]
        public string? Icono { get; set; }

        [Display(Name = "Orden")]
        public int Orden { get; set; } = 0;

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [ForeignKey("MenuPadreID")]
        public virtual Menu? MenuPadre { get; set; }
    }
}
