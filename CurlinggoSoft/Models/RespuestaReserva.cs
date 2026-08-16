using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("RespuestasReserva")]
    public class RespuestaReserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Respuesta")]
        public long RespuestaReservaID { get; set; }

        [Required]
        [Display(Name = "Reserva")]
        public long ReservaID { get; set; }

        [Required(ErrorMessage = "La pregunta es obligatoria")]
        [Display(Name = "Pregunta")]
        public int PreguntaServicioID { get; set; }

        [Display(Name = "Opción")]
        public int? OpcionPreguntaID { get; set; }

        [StringLength(2000)]
        [Display(Name = "Respuesta")]
        public string? RespuestaTexto { get; set; }

        [Display(Name = "Fecha Respuesta")]
        public DateTime FechaRespuesta { get; set; } = DateTime.Now;

        public SolicitudReserva? Reserva { get; set; }
        public PreguntaServicio? Pregunta { get; set; }
        public OpcionPregunta? Opcion { get; set; }
    }
}
