namespace MonitoreoREST.Models.Dtos.Employees
{
    public class Consultar
    {
        public int EmployeeId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
    }
}
