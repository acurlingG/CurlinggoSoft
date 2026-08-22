using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CurlinggoSoft.Models
{
    [Table("Servicios")]
    public class Servicio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServicioID { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public int CategoriaID { get; set; }

        [Required(ErrorMessage = "La subcategoría es obligatoria")]
        [Display(Name = "Subcategoría")]
        public int SubcategoriaID { get; set; }

        [Required(ErrorMessage = "El nombre del servicio es obligatorio")]
        [StringLength(150)]
        [Display(Name = "Nombre")]
        public string NombreServicio { get; set; } = null!;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La tarifa base es obligatoria")]
        [Range(0, double.MaxValue, ErrorMessage = "La tarifa no puede ser negativa")]
        [Display(Name = "Tarifa Diagnóstico Base")]
        public decimal TarifaDiagnosticoBase { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El tiempo debe ser mayor a 0")]
        [Display(Name = "Tiempo Estimado (minutos)")]
        public int TiempoEstimadoMinutos { get; set; } = 60;

        [StringLength(3)]
        [Display(Name = "Moneda")]
        public string Moneda { get; set; } = "CRC";

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        // Relaciones
        [ForeignKey("CategoriaID")]
        [ValidateNever]
        public virtual CategoriaServicio Categoria { get; set; } = null!;

        [ForeignKey("SubcategoriaID")]
        [ValidateNever]
        public virtual SubcategoriaServicio Subcategoria { get; set; } = null!;
    }
}
