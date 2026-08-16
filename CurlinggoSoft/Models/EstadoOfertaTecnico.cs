using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("EstadosOfertaTecnico")]
    public class EstadoOfertaTecnico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Estado Oferta")]
        public int EstadoOfertaID { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(20)]
        public string Codigo { get; set; } = null!;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50)]
        public string Nombre { get; set; } = null!;
    }
}
