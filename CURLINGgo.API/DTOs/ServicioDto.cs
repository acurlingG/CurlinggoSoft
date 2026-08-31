namespace CURLINGgo.API.DTOs
{
    public class CategoriaServicioDto
    {
        public int CategoriaID { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    public class ServicioDto
    {
        public int ServicioID { get; set; }
        public string NombreServicio { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal TarifaDiagnosticoBase { get; set; }
        public int CategoriaID { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public string Moneda { get; set; } = "CRC";
    }
}