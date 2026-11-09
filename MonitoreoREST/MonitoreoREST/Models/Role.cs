using System;
using System.Collections.Generic;

namespace MonitoreoREST.Models;

public partial class Role
{
    public int RolId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
