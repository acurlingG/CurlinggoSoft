using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [StringLength(450)]
        [Display(Name = "ID Usuario")]
        public string UsuarioID { get; set; } = null!;

        [Required(ErrorMessage = "El email es obligatorio")]
        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100)]
        public string Apellidos { get; set; } = null!;

        [StringLength(30)]
        public string? Telefono { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string EstadoUsuario { get; set; } = "Activo";

        [Display(Name = "Fecha Creación")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Display(Name = "Último Acceso")]
        [DataType(DataType.DateTime)]
        public DateTime? UltimoAcceso { get; set; }
    }
}
