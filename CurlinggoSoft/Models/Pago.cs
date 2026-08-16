using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("Pagos")]
    public class Pago
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Pago")]
        public long PagoID { get; set; }

        [Required]
        [Display(Name = "Reserva")]
        public long ReservaID { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        [Display(Name = "Monto Total")]
        public decimal MontoTotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        [Display(Name = "Comisión Plataforma")]
        public decimal ComisionPlataforma { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        [Display(Name = "Monto Neto Técnico")]
        public decimal MontoNetoTecnico { get; set; }

        [Required]
        [StringLength(3)]
        [Display(Name = "Moneda")]
        public string Moneda { get; set; } = "CRC";

        [StringLength(50)]
        [Display(Name = "Proveedor de Pago")]
        public string? ProveedorPago { get; set; }

        [Display(Name = "Clave Idempotencia")]
        public Guid? IdempotencyKey { get; set; }

        [Display(Name = "Fecha Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public SolicitudReserva? Reserva { get; set; }
    }
}
