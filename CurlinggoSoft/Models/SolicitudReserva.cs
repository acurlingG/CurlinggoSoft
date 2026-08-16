using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("SolicitudesReserva")]
    public class SolicitudReserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Reserva")]
        public long ReservaID { get; set; }

        [Display(Name = "Código Seguimiento")]
        public Guid CodigoSeguimiento { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "El cliente es obligatorio")]
        [StringLength(450)]
        [Display(Name = "Cliente")]
        public string ClienteID { get; set; } = null!;

        [StringLength(450)]
        [Display(Name = "Técnico")]
        public string? TecnicoID { get; set; }

        [Required(ErrorMessage = "El servicio es obligatorio")]
        [Display(Name = "Servicio")]
        public int ServicioID { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        public int EstadoReservaID { get; set; }

        [Display(Name = "Dirección")]
        public long? DireccionID { get; set; }

        [Display(Name = "Provincia")]
        public int? ProvinciaID { get; set; }

        [Display(Name = "Cantón")]
        public int? CantonID { get; set; }

        [Display(Name = "Distrito")]
        public int? DistritoID { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        [Display(Name = "Monto Base Cotizado")]
        public decimal MontoBaseCotizado { get; set; }

        [Display(Name = "Duración Estimada (min)")]
        public int DuracionEstimadaMinutos { get; set; } = 60;

        [Required]
        [Display(Name = "Fecha Programada")]
        public DateTime FechaHoraProgramada { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal? LatitudServicio { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal? LongitudServicio { get; set; }

        [Display(Name = "Fecha Solicitud")]
        public DateTime FechaHoraSolicitud { get; set; } = DateTime.Now;

        [Display(Name = "Fecha Completada")]
        public DateTime? FechaHoraCompletada { get; set; }

        [Required(ErrorMessage = "La dirección del servicio es obligatoria")]
        [StringLength(300)]
        [Display(Name = "Dirección del Servicio")]
        public string DireccionServicio { get; set; } = null!;

        [Required]
        [StringLength(2000)]
        [Display(Name = "Descripción Problema")]
        public string DescripcionProblema { get; set; } = "Pendiente de descripción";

        [StringLength(1000)]
        [Display(Name = "Notas Cliente")]
        public string? NotasCliente { get; set; }

        [Display(Name = "Fecha Modificación")]
        public DateTime? FechaModificacion { get; set; }

        [ForeignKey("ClienteID")]
        public virtual ClientePerfil? Cliente { get; set; }

        [ForeignKey("TecnicoID")]
        public virtual TecnicoPerfil? Tecnico { get; set; }

        [ForeignKey("ServicioID")]
        public virtual Servicio? Servicio { get; set; }

        [ForeignKey("EstadoReservaID")]
        public virtual EstadoReserva? EstadoReserva { get; set; }
    }
}
