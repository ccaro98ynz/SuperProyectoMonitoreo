namespace MonitoreoREST.Models.Dtos.Componentes
{
    public class Crear
    {
        public string Codigo { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string? Marca { get; set; }

        public int IdCategoria { get; set; }

        public string? UnidadMedida { get; set; }

        public string? Descripcion { get; set; }

        public decimal? StockMinimo { get; set; }

        public decimal? StockMaximo { get; set; }
        public IFormFile? Imagen { get; set; }
    }
}
