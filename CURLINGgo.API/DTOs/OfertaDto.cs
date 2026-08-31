namespace CURLINGgo.API.DTOs
{
    public class OfertaTecnicoResumenDto
    {
        public long OfertaTecnicoID { get; set; }
        public long ReservaID { get; set; }
        public string ServicioNombre { get; set; } = string.Empty;
        public DateTime FechaHoraProgramada { get; set; }
        public string DireccionServicio { get; set; } = string.Empty;
        public decimal? DistanciaMetros { get; set; }
        public decimal MontoTotalCotizado { get; set; }
        public string DescripcionProblema { get; set; } = string.Empty;
        public int EstadoOfertaID { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime? FechaExpiracion { get; set; }
    }
}