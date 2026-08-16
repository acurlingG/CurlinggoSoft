using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("Evaluaciones")]
    public class Evaluacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Evaluación")]
        public long EvaluacionID { get; set; }

        [Required]
        [Display(Name = "Reserva")]
        public long ReservaID { get; set; }

        [Required(ErrorMessage = "El evaluador es obligatorio")]
        [StringLength(450)]
        [Display(Name = "Evaluador")]
        public string EvaluadorUsuarioID { get; set; } = null!;

        [StringLength(450)]
        [Display(Name = "Evaluado")]
        public string? EvaluadoUsuarioID { get; set; }

        [Display(Name = "Servicio")]
        public int? ServicioID { get; set; }

        [Required(ErrorMessage = "El tipo de evaluación es obligatorio")]
        [Display(Name = "Tipo de Evaluación")]
        public int TipoEvaluacionID { get; set; }

        [Required]
        [Range(1, 5)]
        [Display(Name = "Puntuación")]
        public byte Puntuacion { get; set; }

        [StringLength(1000)]
        public string? Comentario { get; set; }

        [Display(Name = "Fecha Evaluación")]
        public DateTime FechaEvaluacion { get; set; } = DateTime.Now;

        [Display(Name = "Activa")]
        public bool Activa { get; set; } = true;

        public SolicitudReserva? Reserva { get; set; }
        public Usuario? Evaluador { get; set; }
        public Usuario? Evaluado { get; set; }
        public Servicio? Servicio { get; set; }
        public TipoEvaluacion? TipoEvaluacion { get; set; }
    }
}
