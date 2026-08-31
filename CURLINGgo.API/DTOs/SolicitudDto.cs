namespace CURLINGgo.API.DTOs
{
    public class CrearSolicitudDto
    {
        public int ServicioID { get; set; }

        // AGREGAR: Campo requerido por la restricción CK_Reservas_DireccionCompleta
        public int DireccionID { get; set; }

        public DateTime FechaHoraProgramada { get; set; }
        public string DireccionServicio { get; set; } = string.Empty;
        public int? ProvinciaID { get; set; }
        public int? CantonID { get; set; }
        public int? DistritoID { get; set; }
        public string DescripcionProblema { get; set; } = string.Empty;
        public decimal? LatitudServicio { get; set; }
        public decimal? LongitudServicio { get; set; }
        public string? NotasCliente { get; set; }
    }

    public class SolicitudResumenDto
    {
        public long ReservaID { get; set; }
        public Guid CodigoSeguimiento { get; set; }
        public string ServicioNombre { get; set; } = string.Empty;
        public DateTime FechaHoraProgramada { get; set; }
        public int EstadoReservaID { get; set; }
        public string EstadoNombre { get; set; } = string.Empty;
        public string DireccionServicio { get; set; } = string.Empty;
        public decimal MontoTotalCotizado { get; set; }
        public string? TecnicoID { get; set; }
    }

    public class SolicitudDetalleDto
    {
        public long ReservaID { get; set; }
        public Guid CodigoSeguimiento { get; set; }
        public int ServicioID { get; set; }
        public string ServicioNombre { get; set; } = string.Empty;
        public string ClienteID { get; set; } = string.Empty;
        public DateTime FechaHoraProgramada { get; set; }
        public DateTime FechaHoraSolicitud { get; set; }
        public int EstadoReservaID { get; set; }
        public string EstadoNombre { get; set; } = string.Empty;
        public long? DireccionID { get; set; }
        public string DireccionServicio { get; set; } = string.Empty;
        public int? ProvinciaID { get; set; }
        public int? CantonID { get; set; }
        public int? DistritoID { get; set; }
        public string DescripcionProblema { get; set; } = string.Empty;
        public decimal? LatitudServicio { get; set; }
        public decimal? LongitudServicio { get; set; }
        public string? NotasCliente { get; set; }
        public decimal MontoBaseCotizado { get; set; }
        public decimal MontoAjustes { get; set; }
        public decimal MontoTotalCotizado { get; set; }
        public string Moneda { get; set; } = "CRC";
        public string? TecnicoID { get; set; }
    }
    
    public class CambiarEstadoSolicitudDto
    {
        public int NuevoEstadoID { get; set; }
        public string? Observaciones { get; set; }
    }
}