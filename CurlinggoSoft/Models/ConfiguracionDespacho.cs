using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    // Tabla de configuracion (fila unica) para el motor de despacho
    // automatico de tecnicos. Permite al administrador ajustar desde el
    // sistema, sin tocar codigo ni el procedimiento almacenado, el radio
    // de busqueda y el maximo de tecnicos candidatos que usa
    // usp_Reserva_BuscarTecnicosDisponibles.
    [Table("ConfiguracionDespacho")]
    public class ConfiguracionDespacho
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ConfiguracionDespachoID { get; set; } = 1;

        [Required(ErrorMessage = "El radio de busqueda en km es obligatorio")]
        [Range(0.1, 500, ErrorMessage = "El radio debe estar entre 0.1 y 500 km")]
        [Column(TypeName = "decimal(8,2)")]
        [Display(Name = "Radio de búsqueda (km)")]
        public decimal RadioKm { get; set; } = 20.00m;

        [Required(ErrorMessage = "El máximo de técnicos es obligatorio")]
        [Range(1, 100, ErrorMessage = "El máximo de técnicos debe estar entre 1 y 100")]
        [Display(Name = "Máximo de técnicos por búsqueda")]
        public int MaxTecnicos { get; set; } = 10;

        [Display(Name = "Última actualización")]
        [DataType(DataType.DateTime)]
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
    }
}
