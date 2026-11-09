using System;
using System.Collections.Generic;

namespace MonitoreoREST.Models;

public partial class Proveedore
{
    public int IdProveedor { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<OrdenesCompra> OrdenesCompras { get; set; } = new List<OrdenesCompra>();

    public virtual ICollection<ProveedoresComponente> ProveedoresComponentes { get; set; } = new List<ProveedoresComponente>();
}
