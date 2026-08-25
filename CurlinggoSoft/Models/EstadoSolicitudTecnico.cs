using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    // Catálogo de estados del flujo de registro/aprobación de técnicos.
    // Códigos esperados: BORRADOR, ENVIADA, EN_REVISION, INFO_REQUERIDA,
    // BACKGROUND_PENDIENTE, BACKGROUND_EN_PROCESO, APROBADA, RECHAZADA, CANCELADA.
    [Table("EstadosSolicitudTecnico")]
    public class EstadoSolicitudTecnico
    {
        [Key]
        [Display(Name = "ID Estado")]
        public int EstadoSolicitudTecnicoID { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(30)]
        public string Codigo { get; set; } = null!;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = null!;

        [StringLength(300)]
        public string? Descripcion { get; set; }

        [Display(Name = "Orden en Flujo")]
        public int Orden { get; set; }

        public bool Activo { get; set; } = true;
    }
}
