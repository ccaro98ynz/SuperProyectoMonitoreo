using System;
using System.Collections.Generic;

namespace MonitoreoREST.Models;

public partial class OrdenesCompraDetalle
{
    public int IdDetalle { get; set; }

    public int IdOrdenCompra { get; set; }

    public int IdComponente { get; set; }

    public int CantidadOrdenada { get; set; }

    public decimal PrecioUnitario { get; set; }

    public virtual Componente IdComponenteNavigation { get; set; } = null!;

    public virtual OrdenesCompra IdOrdenCompraNavigation { get; set; } = null!;

    public virtual ICollection<RecepcionDetalle> RecepcionDetalles { get; set; } = new List<RecepcionDetalle>();
}
