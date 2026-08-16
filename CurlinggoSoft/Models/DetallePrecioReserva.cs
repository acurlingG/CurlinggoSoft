using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("DetallePrecioReserva")]
    public class DetallePrecioReserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Detalle")]
        public long DetallePrecioReservaID { get; set; }

        [Required]
        [Display(Name = "Reserva")]
        public long ReservaID { get; set; }

        [Required(ErrorMessage = "El concepto es obligatorio")]
        [StringLength(150)]
        [Display(Name = "Concepto")]
        public string Concepto { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }

        [Display(Name = "Opción")]
        public int? OpcionPreguntaID { get; set; }

        [Display(Name = "Fecha Registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public SolicitudReserva? Reserva { get; set; }
        public OpcionPregunta? Opcion { get; set; }
    }
}
