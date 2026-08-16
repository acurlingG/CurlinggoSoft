using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("EstadosPago")]
    public class EstadoPago
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EstadoPagoID { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(30)]
        public string Codigo { get; set; } = null!;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = null!;
    }
}
