namespace MonitoreoREST.Models.Dtos.Orders
{
    public class NuevoPedido
    {
        public int IdProveedor { get; set; }
        public List<ContenidoPedido> Contenido { get; set; }
    }
}
