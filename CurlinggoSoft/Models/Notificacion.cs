using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("Notificaciones")]
    public class Notificacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Notificación")]
        public long NotificacionID { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(450)]
        [Display(Name = "Usuario")]
        public string UsuarioID { get; set; } = null!;

        [Display(Name = "Reserva")]
        public long? ReservaID { get; set; }

        [Display(Name = "Oferta")]
        public long? OfertaTecnicoID { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Tipo")]
        public string TipoNotificacion { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = null!;

        [Required]
        [StringLength(1000)]
        public string Mensaje { get; set; } = null!;

        [Display(Name = "Leída")]
        public bool Leida { get; set; } = false;

        [Display(Name = "Fecha Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Display(Name = "Fecha Lectura")]
        public DateTime? FechaLectura { get; set; }

        public Usuario? Usuario { get; set; }
        public SolicitudReserva? Reserva { get; set; }
        public OfertaTecnico? OfertaTecnico { get; set; }
    }
}
