using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    // Catálogo de tipos de documento que un aspirante puede/debe subir
    // (IDENTIFICACION, LICENCIA, CERTIFICACION, SEGURO, OTRO).
    [Table("TiposDocumentoTecnico")]
    public class TipoDocumentoTecnico
    {
        [Key]
        [Display(Name = "ID Tipo Documento")]
        public int TipoDocumentoID { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(30)]
        public string Codigo { get; set; } = null!;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = null!;

        [StringLength(300)]
        public string? Descripcion { get; set; }

        [Display(Name = "Obligatorio")]
        public bool Obligatorio { get; set; } = false;

        public bool Activo { get; set; } = true;
    }
}
