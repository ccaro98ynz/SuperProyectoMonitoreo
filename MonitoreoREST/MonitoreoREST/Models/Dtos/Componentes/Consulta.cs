namespace MonitoreoREST.Models.Dtos.Componentes
{
    public class Consulta
    {
        public int IdComponente { get; set; }
        public string? Codigo { get; set; } 
        public string? Nombre { get; set; } 
        public string? Marca { get; set; } 
        public string? Categoria { get; set; } 
        public string? UnidadMedida { get; set; } 
        public string? Descripcion { get; set; }
        public decimal? StockMinimo { get; set; }
        public decimal? StockMaximo { get; set; }
        public string? ImagenUrl { get; set; }

    }
}
