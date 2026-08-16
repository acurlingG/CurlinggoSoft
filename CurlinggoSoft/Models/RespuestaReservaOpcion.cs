using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("RespuestasReservaOpciones")]
    public class RespuestaReservaOpcion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public long RespuestaReservaOpcionID { get; set; }

        [Required]
        [Display(Name = "Respuesta")]
        public long RespuestaReservaID { get; set; }

        [Required(ErrorMessage = "La opción es obligatoria")]
        [Display(Name = "Opción")]
        public int OpcionPreguntaID { get; set; }

        public RespuestaReserva? RespuestaReserva { get; set; }
        public OpcionPregunta? Opcion { get; set; }
    }
}
