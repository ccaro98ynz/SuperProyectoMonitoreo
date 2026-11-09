using System;
using System.Collections.Generic;

namespace MonitoreoREST.Models;

public partial class ProveedoresComponente
{
    public int IdProveedor { get; set; }

    public int IdComponente { get; set; }

    public decimal? Precio { get; set; }

    public string? CodigoProveedor { get; set; }

    public bool Activo { get; set; }

    public virtual Componente IdComponenteNavigation { get; set; } = null!;

    public virtual Proveedore IdProveedorNavigation { get; set; } = null!;
}
