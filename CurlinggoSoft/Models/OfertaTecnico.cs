using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("OfertasTecnico")]
    public class OfertaTecnico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Oferta")]
        public long OfertaTecnicoID { get; set; }

        [Required]
        [Display(Name = "Reserva")]
        public long ReservaID { get; set; }

        [Required(ErrorMessage = "El técnico es obligatorio")]
        [StringLength(450)]
        [Display(Name = "Técnico")]
        public string TecnicoID { get; set; } = null!;

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        public int EstadoOfertaID { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        [Display(Name = "Distancia (m)")]
        public decimal? DistanciaMetros { get; set; }

        [Display(Name = "Orden")]
        public int? OrdenOferta { get; set; }

        [Display(Name = "Fecha Envío")]
        public DateTime FechaEnvio { get; set; } = DateTime.Now;

        [Display(Name = "Fecha Expiración")]
        public DateTime? FechaExpiracion { get; set; }

        [Display(Name = "Fecha Respuesta")]
        public DateTime? FechaRespuesta { get; set; }

        [StringLength(500)]
        public string? Mensaje { get; set; }

        public SolicitudReserva? Reserva { get; set; }
        public TecnicoPerfil? Tecnico { get; set; }
        public EstadoOfertaTecnico? EstadoOferta { get; set; }
    }
}
