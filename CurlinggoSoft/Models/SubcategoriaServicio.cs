using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("SubcategoriasServicio")]
    public class SubcategoriaServicio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubcategoriaID { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public int CategoriaID { get; set; }

        [Required(ErrorMessage = "El nombre de la subcategoría es obligatorio")]
        [StringLength(120)]
        [Display(Name = "Nombre")]
        public string NombreSubcategoria { get; set; } = null!;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Display(Name = "Activa")]
        public bool Activa { get; set; } = true;

        // Relaciones
        [ForeignKey("CategoriaID")]
        public virtual CategoriaServicio Categoria { get; set; } = null!;
        public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
    }
}
