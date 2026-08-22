using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    // Mensaje de chat entre Cliente y Técnico, atado a una reserva.
    // Se permite enviar/leer solo mientras la reserva está en un estado
    // activo (ASIGNADA, EN_CAMINO, EN_PROCESO) — esa regla se valida en el
    // controlador (MensajesController), no aquí.
    [Table("MensajesReserva")]
    public class MensajeReserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Mensaje")]
        public long MensajeID { get; set; }

        [Required]
        [Display(Name = "Reserva")]
        public long ReservaID { get; set; }

        [Required(ErrorMessage = "El emisor es obligatorio")]
        [StringLength(450)]
        [Display(Name = "Emisor")]
        public string EmisorUsuarioID { get; set; } = null!;

        [Required(ErrorMessage = "El receptor es obligatorio")]
        [StringLength(450)]
        [Display(Name = "Receptor")]
        public string ReceptorUsuarioID { get; set; } = null!;

        [Required(ErrorMessage = "El mensaje no puede estar vacío")]
        [StringLength(1000)]
        public string Texto { get; set; } = null!;

        [Display(Name = "Fecha Envío")]
        public DateTime FechaEnvio { get; set; } = DateTime.Now;

        [Display(Name = "Leído")]
        public bool Leido { get; set; } = false;

        [ForeignKey("ReservaID")]
        public virtual SolicitudReserva? Reserva { get; set; }

        [ForeignKey("EmisorUsuarioID")]
        public virtual Usuario? Emisor { get; set; }

        [ForeignKey("ReceptorUsuarioID")]
        public virtual Usuario? Receptor { get; set; }
    }
}
