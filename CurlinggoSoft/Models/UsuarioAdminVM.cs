using System.ComponentModel.DataAnnotations;

namespace CurlinggoSoft.Models
{
    // ViewModel usado por UsuariosController para dar de alta/editar un
    // usuario combinando los datos de autenticacion (ASP.NET Identity:
    // AspNetUsers / AspNetRoles) con el perfil de negocio (tabla Usuarios).
    // Esto evita duplicar el flujo: se crea 1 sola vez en Identity y se
    // replica el Id hacia Usuarios.UsuarioID.
    public class UsuarioAdminVM
    {
        // Nulo en Create (se genera al crear el IdentityUser); presente en Edit.
        public string? UsuarioID { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; } = null!;

        // Solo requerida al crear. En Edit, si se deja en blanco, no se cambia la clave.
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string? Clave { get; set; }

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

        [Required(ErrorMessage = "El rol es obligatorio")]
        [Display(Name = "Rol")]
        public string Rol { get; set; } = null!;

        public IEnumerable<string> RolesDisponibles { get; set; } = Array.Empty<string>();
    }
}
