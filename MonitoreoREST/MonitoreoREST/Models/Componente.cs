using System;
using System.Collections.Generic;

namespace MonitoreoREST.Models;

public partial class Componente
{
    public int IdComponente { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Marca { get; set; }

    public int IdCategoria { get; set; }

    public string? UnidadMedida { get; set; }

    public string? Descripcion { get; set; }

    public decimal? StockMinimo { get; set; }

    public decimal? StockMaximo { get; set; }

    public string? ImagenUrl { get; set; }

    public bool? Activo { get; set; }

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual Inventario? Inventario { get; set; }

    public virtual ICollection<OrdenesCompraDetalle> OrdenesCompraDetalles { get; set; } = new List<OrdenesCompraDetalle>();

    public virtual ICollection<ProveedoresComponente> ProveedoresComponentes { get; set; } = new List<ProveedoresComponente>();

    public virtual ICollection<RecepcionDetalle> RecepcionDetalles { get; set; } = new List<RecepcionDetalle>();
}
