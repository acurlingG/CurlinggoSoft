using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("HistorialEstadosReserva")]
    public class HistorialEstadoReserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Historial")]
        public long HistorialID { get; set; }

        [Required(ErrorMessage = "La reserva es obligatoria")]
        [Display(Name = "Reserva")]
        public long ReservaID { get; set; }

        [Display(Name = "Estado Anterior")]
        public int? EstadoAnteriorID { get; set; }

        [Required(ErrorMessage = "El estado nuevo es obligatorio")]
        [Display(Name = "Estado Nuevo")]
        public int EstadoNuevoID { get; set; }

        [Display(Name = "Fecha Cambio")]
        public DateTime FechaCambio { get; set; } = DateTime.Now;

        [StringLength(450)]
        [Display(Name = "Usuario Modificador")]
        public string? UsuarioModificadorID { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }

        [ForeignKey("ReservaID")]
        public virtual SolicitudReserva? Reserva { get; set; }

        [ForeignKey("EstadoAnteriorID")]
        public virtual EstadoReserva? EstadoAnterior { get; set; }

        [ForeignKey("EstadoNuevoID")]
        public virtual EstadoReserva? EstadoNuevo { get; set; }

        [ForeignKey("UsuarioModificadorID")]
        public virtual Usuario? UsuarioModificador { get; set; }
    }
}
