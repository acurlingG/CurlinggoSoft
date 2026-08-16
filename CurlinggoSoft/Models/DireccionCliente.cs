using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("DireccionesCliente")]
    public class DireccionCliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Dirección")]
        public long DireccionID { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio")]
        [StringLength(450)]
        [Display(Name = "Cliente")]
        public string ClienteID { get; set; } = null!;

        [Required(ErrorMessage = "El nombre de dirección es obligatorio")]
        [StringLength(80)]
        [Display(Name = "Nombre Dirección")]
        public string NombreDireccion { get; set; } = null!;

        [Required(ErrorMessage = "La provincia es obligatoria")]
        [Display(Name = "Provincia")]
        public int ProvinciaID { get; set; }

        [Required(ErrorMessage = "El cantón es obligatorio")]
        [Display(Name = "Cantón")]
        public int CantonID { get; set; }

        [Required(ErrorMessage = "El distrito es obligatorio")]
        [Display(Name = "Distrito")]
        public int DistritoID { get; set; }

        [Required(ErrorMessage = "La dirección exacta es obligatoria")]
        [StringLength(300)]
        [Display(Name = "Dirección Exacta")]
        public string DireccionExacta { get; set; } = null!;

        [Column(TypeName = "decimal(9,6)")]
        public decimal? Latitud { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal? Longitud { get; set; }

        [Display(Name = "Es Principal")]
        public bool EsPrincipal { get; set; } = false;

        [Display(Name = "Activa")]
        public bool Activa { get; set; } = true;

        [Display(Name = "Fecha Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [ForeignKey("ClienteID")]
        public virtual ClientePerfil? Cliente { get; set; }

        [ForeignKey("ProvinciaID")]
        public virtual Provincia? Provincia { get; set; }

        [ForeignKey("CantonID")]
        public virtual Canton? Canton { get; set; }

        [ForeignKey("DistritoID")]
        public virtual Distrito? Distrito { get; set; }
    }
}
