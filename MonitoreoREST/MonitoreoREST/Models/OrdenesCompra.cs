using System;
using System.Collections.Generic;

namespace MonitoreoREST.Models;

public partial class OrdenesCompra
{
    public int IdOrdenCompra { get; set; }

    public string FolioOrden { get; set; } = null!;

    public int IdProveedor { get; set; }

    public DateTime FechaOrden { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime? FechaRecibo { get; set; }

    public DateTime? FechaEsperadaRecibo { get; set; }

    public virtual Proveedore IdProveedorNavigation { get; set; } = null!;

    public virtual ICollection<OrdenesCompraDetalle> OrdenesCompraDetalles { get; set; } = new List<OrdenesCompraDetalle>();

    public virtual ICollection<Recepcione> Recepciones { get; set; } = new List<Recepcione>();
}
