using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("PreguntasServicio")]
    public class PreguntaServicio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Pregunta")]
        public int PreguntaServicioID { get; set; }

        [Required(ErrorMessage = "El servicio es obligatorio")]
        [Display(Name = "Servicio")]
        public int ServicioID { get; set; }

        [Required(ErrorMessage = "El texto es obligatorio")]
        [StringLength(500)]
        [Display(Name = "Texto Pregunta")]
        public string TextoPregunta { get; set; } = null!;

        [Required]
        [StringLength(20)]
        [Display(Name = "Tipo Respuesta")]
        public string TipoRespuesta { get; set; } = null!;

        [Display(Name = "Obligatoria")]
        public bool Obligatoria { get; set; } = true;

        [Display(Name = "Orden")]
        public int Orden { get; set; } = 1;

        [Display(Name = "Activa")]
        public bool Activa { get; set; } = true;

        [ForeignKey("ServicioID")]
        public virtual Servicio? Servicio { get; set; }
    }
}
