using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("IntentosPago")]
    public class IntentoPago
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Intento")]
        public long IntentoPagoID { get; set; }

        [Required]
        [Display(Name = "Pago")]
        public long PagoID { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        [Display(Name = "Método de Pago")]
        public int MetodoPagoID { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        public int EstadoPagoID { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        [Display(Name = "Monto del Intento")]
        public decimal MontoIntento { get; set; }

        [StringLength(150)]
        [Display(Name = "Referencia Comprobante")]
        public string? ReferenciaComprobante { get; set; }

        [StringLength(200)]
        [Display(Name = "Referencia Proveedor")]
        public string? ReferenciaProveedor { get; set; }

        [Display(Name = "Fecha Intento")]
        public DateTime FechaIntento { get; set; } = DateTime.Now;

        [StringLength(500)]
        [Display(Name = "Mensaje Proveedor")]
        public string? MensajeProveedor { get; set; }

        public Pago? Pago { get; set; }
        public MetodoPago? MetodoPago { get; set; }
        public EstadoPago? EstadoPago { get; set; }
    }
}
