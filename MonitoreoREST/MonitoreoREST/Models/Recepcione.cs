using System;
using System.Collections.Generic;

namespace MonitoreoREST.Models;

public partial class Recepcione
{
    public int IdRecepcion { get; set; }

    public int IdOrdenCompra { get; set; }

    public DateTime FechaRecepcion { get; set; }

    public string? Estado { get; set; }

    public string? Observaciones { get; set; }

    public virtual OrdenesCompra IdOrdenCompraNavigation { get; set; } = null!;

    public virtual ICollection<RecepcionDetalle> RecepcionDetalles { get; set; } = new List<RecepcionDetalle>();
}
