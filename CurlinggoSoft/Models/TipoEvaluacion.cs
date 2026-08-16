using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("TiposEvaluacion")]
    public class TipoEvaluacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TipoEvaluacionID { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(40)]
        public string Codigo { get; set; } = null!;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = null!;
    }
}
