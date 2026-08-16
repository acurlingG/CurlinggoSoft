using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("LogsIntervencionOperativa")]
    public class LogIntervencionOperativa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Log")]
        public long LogID { get; set; }

        [Display(Name = "Reserva")]
        public long? ReservaID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Tipo de Evento")]
        public string TipoEvento { get; set; } = null!;

        [Display(Name = "Datos de Entrada")]
        public string? DatosEntradaJson { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Decisión Tomada")]
        public string DecisionTomada { get; set; } = null!;

        [StringLength(100)]
        [Display(Name = "Versión de Modelo")]
        public string? ModeloVersion { get; set; }

        [StringLength(450)]
        [Display(Name = "Usuario Intervención")]
        public string? UsuarioIntervencionID { get; set; }

        [Display(Name = "Fecha Registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public SolicitudReserva? Reserva { get; set; }
        public Usuario? UsuarioIntervencion { get; set; }
    }
}
