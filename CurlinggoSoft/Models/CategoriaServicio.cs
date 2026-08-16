using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("CategoriasServicio")]
    public class CategoriaServicio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoriaID { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string NombreCategoria { get; set; } = null!;

        [StringLength(255)]
        public string? Descripcion { get; set; }

        [Display(Name = "Activa")]
        public bool Activa { get; set; } = true;

        // Relaciones
        public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
        public virtual ICollection<SubcategoriaServicio> Subcategorias { get; set; } = new List<SubcategoriaServicio>();
    }
}
