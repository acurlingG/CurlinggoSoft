using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    // Especialidades declaradas por el aspirante durante el wizard (paso 3).
    // Al aprobarse la solicitud, estos registros se copian a TecnicoEspecialidades
    // mediante usp_SolicitudTecnico_Aprobar.
    [Table("SolicitudTecnicoEspecialidades")]
    public class SolicitudTecnicoEspecialidad
    {
        [Key]
        [Display(Name = "ID Especialidad")]
        public long SolicitudTecnicoEspecialidadID { get; set; }

        [Required]
        [Display(Name = "Solicitud")]
        public long SolicitudTecnicoID { get; set; }

        [Required]
        [Display(Name = "Servicio")]
        public int ServicioID { get; set; }

        [Range(0, 60, ErrorMessage = "Los años de experiencia deben estar entre 0 y 60")]
        [Display(Name = "Años Experiencia")]
        public int AniosExperiencia { get; set; } = 0;

        [StringLength(1000)]
        [Display(Name = "Descripción de Experiencia")]
        public string? DescripcionExperiencia { get; set; }

        [ForeignKey("SolicitudTecnicoID")]
        public virtual SolicitudTecnico? Solicitud { get; set; }

        [ForeignKey("ServicioID")]
        public virtual Servicio? Servicio { get; set; }
    }
}
