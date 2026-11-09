using System;
using System.Collections.Generic;

namespace MonitoreoREST.Models;

public partial class Inventario
{
    public int IdInventario { get; set; }

    public int IdComponente { get; set; }

    public decimal CantidadDisponible { get; set; }

    public DateTime FechaActualizacion { get; set; }

    public virtual Componente IdComponenteNavigation { get; set; } = null!;
}
