using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("TecnicoEspecialidades")]
    public class TecnicoEspecialidad
    {
        [Display(Name = "Técnico")]
        public string TecnicoID { get; set; } = null!;

        [Display(Name = "Servicio")]
        public int ServicioID { get; set; }

        [Display(Name = "Años Experiencia")]
        public int AniosExperiencia { get; set; } = 1;

        [ForeignKey("TecnicoID")]
        public virtual TecnicoPerfil? Tecnico { get; set; }

        [ForeignKey("ServicioID")]
        public virtual Servicio? Servicio { get; set; }
    }
}
