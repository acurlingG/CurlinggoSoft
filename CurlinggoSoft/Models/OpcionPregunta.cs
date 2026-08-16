using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("OpcionesPregunta")]
    public class OpcionPregunta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Opción")]
        public int OpcionPreguntaID { get; set; }

        [Required(ErrorMessage = "La pregunta es obligatoria")]
        [Display(Name = "Pregunta")]
        public int PreguntaServicioID { get; set; }

        [Required(ErrorMessage = "El texto de opción es obligatorio")]
        [StringLength(300)]
        [Display(Name = "Texto Opción")]
        public string TextoOpcion { get; set; } = null!;

        [StringLength(100)]
        public string? Valor { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        [Display(Name = "Ajuste de Precio")]
        public decimal AjustePrecio { get; set; } = 0;

        [Display(Name = "Orden")]
        public int Orden { get; set; } = 1;

        [Display(Name = "Activa")]
        public bool Activa { get; set; } = true;

        [ForeignKey("PreguntaServicioID")]
        public virtual PreguntaServicio? Pregunta { get; set; }
    }
}
