using System;
using System.Collections.Generic;

namespace MonitoreoREST.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public int RolId { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual Role Rol { get; set; } = null!;
}
