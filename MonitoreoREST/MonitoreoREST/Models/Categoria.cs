using System;
using System.Collections.Generic;

namespace MonitoreoREST.Models;

public partial class Categoria
{
    public int IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Componente> Componentes { get; set; } = new List<Componente>();
}
