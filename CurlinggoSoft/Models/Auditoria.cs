using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("Auditoria")]
    public class Auditoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Auditoría")]
        public long AuditoriaID { get; set; }

        [StringLength(450)]
        [Display(Name = "Usuario")]
        public string? UsuarioID { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Tabla Afectada")]
        public string TablaAfectada { get; set; } = null!;

        [StringLength(100)]
        [Display(Name = "Registro")]
        public string? RegistroID { get; set; }

        [Required]
        [StringLength(20)]
        public string Operacion { get; set; } = null!;

        [Display(Name = "Valores Anteriores")]
        public string? ValoresAnterioresJson { get; set; }

        [Display(Name = "Valores Nuevos")]
        public string? ValoresNuevosJson { get; set; }

        [Display(Name = "Fecha Evento")]
        public DateTime FechaEvento { get; set; } = DateTime.Now;

        [StringLength(45)]
        [Display(Name = "Dirección IP")]
        public string? DireccionIP { get; set; }

        [Display(Name = "Correlation ID")]
        public Guid? CorrelationID { get; set; }

        public Usuario? Usuario { get; set; }
    }
}
