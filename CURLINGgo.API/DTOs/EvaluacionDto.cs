using System.ComponentModel.DataAnnotations;

namespace CURLINGgo.API.DTOs
{
    public class CrearEvaluacionDto
    {
        [Required]
        public long ReservaID { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5.")]
        public byte Puntuacion { get; set; } // Tipo byte igual al modelo

        public int TipoEvaluacionID { get; set; } = 1; // 1 = Cliente a Técnico (por defecto)

        [MaxLength(1000)]
        public string? Comentario { get; set; }
    }

    public class EvaluacionDetalleDto
    {
        public long EvaluacionID { get; set; }
        public long ReservaID { get; set; }
        public string EvaluadorUsuarioID { get; set; } = string.Empty;
        public string? EvaluadoUsuarioID { get; set; }
        public byte Puntuacion { get; set; }
        public string? Comentario { get; set; }
        public DateTime FechaEvaluacion { get; set; }
    }
}