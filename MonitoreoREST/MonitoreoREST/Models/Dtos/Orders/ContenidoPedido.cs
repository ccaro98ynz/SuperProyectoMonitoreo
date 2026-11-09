namespace MonitoreoREST.Models.Dtos.Orders
{
    public class ContenidoPedido
    {
        public int CompraId { get; set; }
        public int ComponenteId { get; set; }
        public int CantidadOrdenada { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
