using System;
using System.Collections.Generic;

namespace MonitoreoREST.Models;

public partial class RecepcionDetalle
{
    public int IdRecepcionDetalle { get; set; }

    public int IdRecepcion { get; set; }

    public int IdDetalleOrden { get; set; }

    public decimal CantidadRecibida { get; set; }

    public decimal CantidadMerma { get; set; }

    public decimal CantidadRechazada { get; set; }

    public string? NumeroBatch { get; set; }

    public string? EstadoQa { get; set; }

    public string? Observaciones { get; set; }

    public int? IdComponente { get; set; }

    public virtual Componente? IdComponenteNavigation { get; set; }

    public virtual OrdenesCompraDetalle IdDetalleOrdenNavigation { get; set; } = null!;

    public virtual Recepcione IdRecepcionNavigation { get; set; } = null!;
}
