using BCrypt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoreoREST.Models;
using MonitoreoREST.Models.Dtos.Employees;
using System.Security.Claims;
namespace MonitoreoREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class employeesController : ControllerBase
    {
        private readonly BdMonitoreoContext _context;

        public employeesController(BdMonitoreoContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<IActionResult> RegisterEmployee(Registro EmpleadoNuevo)
        {
            try
            {
                if (EmpleadoNuevo.Password == "") return BadRequest("La contraseña no puede estar vacía");
                if (EmpleadoNuevo.Email is null || EmpleadoNuevo.Password is null) return BadRequest("Se requiere correo electronico y contraseña");
                if (await _context.Employees.AnyAsync(e => e.Email == EmpleadoNuevo.Email)) return Conflict("Email already in use.");
                var employee = new Employee
                {
                    Nombre = EmpleadoNuevo.Nombre,
                    Apellido = EmpleadoNuevo.Apellido,
                    Email = EmpleadoNuevo.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(EmpleadoNuevo.Password, workFactor: 12),
                    RolId = 5 ,// Asignar un rol predeterminado, por ejemplo, 1 para "Usuario"
                    Activo =true
                };
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return StatusCode(201, new
                {
                    id = employee.EmployeeId
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar empleado: {ex.Message}");
                return StatusCode(500, new
                {
                    message = "Error al registrar el empleado",
                    error = ex.Message
                });
            }
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginEmployee([FromBody] Credenciales empleado)
        {
            if (string.IsNullOrWhiteSpace(empleado.Email) ||
                string.IsNullOrWhiteSpace(empleado.Password))
                return BadRequest("Credenciales obligatorias");

            var employee = await _context.Employees
                .Include(e => e.Rol)
                .FirstOrDefaultAsync(e => e.Email == empleado.Email);

            if (employee is null ||
                !employee.Activo ||
                !employee.Rol.Activo ||
                !BCrypt.Net.BCrypt.Verify(empleado.Password, employee.PasswordHash))
                return Unauthorized("Credenciales incorrectas");

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()),
        new Claim(ClaimTypes.Name, employee.Nombre),
        new Claim(ClaimTypes.Role, employee.Rol.Nombre)
    };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity)
            );

            return Ok(new
            {
                message = "Inicio de sesión exitoso",
                id = employee.EmployeeId
            });
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet("consultar")]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees.Select(e => new Consultar
            {
                EmployeeId = e.EmployeeId,
                Nombre = e.Nombre,
                Apellido = e.Apellido,
                Email = e.Email,
                Rol = e.Rol.Nombre
            }).ToListAsync();
            return Ok(employees);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPatch("actualizar-permisos/{id:int}")]
        public async Task<IActionResult> UpdateEmployeePermissions(int id, int nuevoRol)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee is null) return NotFound("Empleado no encontrado");

            if (!await _context.Roles.AnyAsync(r => r.RolId == nuevoRol && r.Activo))
                return BadRequest("El rol no existe o está desactivado");

            employee.RolId = nuevoRol;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Permisos actualizados exitosamente"
            });
        }
    }
}
