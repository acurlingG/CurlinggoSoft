using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("DisponibilidadTecnico")]
    public class DisponibilidadTecnico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Disponibilidad")]
        public long DisponibilidadID { get; set; }

        [Required(ErrorMessage = "El técnico es obligatorio")]
        [StringLength(450)]
        [Display(Name = "Técnico")]
        public string TecnicoID { get; set; } = null!;

        [Required]
        [Range(1, 7, ErrorMessage = "El día debe estar entre 1 (Lunes) y 7 (Domingo)")]
        [Display(Name = "Día Semana (1=Lun...7=Dom)")]
        public byte DiaSemana { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Hora Inicio")]
        public TimeSpan HoraInicio { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Hora Fin")]
        public TimeSpan HoraFin { get; set; }

        [Display(Name = "Activa")]
        public bool Activa { get; set; } = true;

        [ForeignKey("TecnicoID")]
        public virtual TecnicoPerfil? Tecnico { get; set; }
    }
}
