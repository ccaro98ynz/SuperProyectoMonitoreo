using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonitoreoREST.Models;
using MonitoreoREST.Models.Dtos.Orders;
using MonitoreoREST.Models.Dtos.Componentes;

using Microsoft.EntityFrameworkCore;

namespace MonitoreoREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ordersController : ControllerBase
    {
        private readonly BdMonitoreoContext _context;
        public ordersController(BdMonitoreoContext context)
        {
            _context = context;
        }
        [HttpGet("proveedores")]
        public async Task<ActionResult<IEnumerable<ProveedorDto>>> GetProviders()
        {
            var proveedores = await _context.Proveedores
                .AsNoTracking()
                .Select(p => new ProveedorDto
            {
                IdProveedor = p.IdProveedor,
                Nombre = p.Nombre
            }).ToListAsync();
            return Ok(proveedores);
        }
        [HttpGet("componentes/proveedor/{id:int}")]
        public async Task<IActionResult> GetProvidersComponents(int id)
        {
            if (!await _context.Proveedores.AnyAsync(p => p.IdProveedor == id))
                return NotFound("El proveedor no existe.");

            var componentesQueVende = await _context.ProveedoresComponentes
                .AsNoTracking()
                .Where(c => c.IdProveedor == id&& c.Activo == true)
                .Select(c => new ComponentesVenta
                {
                    VentaId = c.IdComponente,
                    Nombre = c.IdComponenteNavigation.Nombre,
                    Proveedor = c.IdProveedorNavigation.Nombre
                })
                .ToListAsync();

            return Ok(componentesQueVende);
        }
        [HttpPost("realizar-pedido")]
        public async Task<IActionResult> MakeOrder([FromBody] NuevoPedido pedidoEntrante)
        {
            if (!await _context.Proveedores.AnyAsync(p => p.IdProveedor == pedidoEntrante.IdProveedor))
                return NotFound("El proveedor no existe.");

            if (pedidoEntrante.Contenido is null || pedidoEntrante.Contenido.Count == 0)
                return BadRequest("El pedido debe contener al menos un componente.");

            if (pedidoEntrante.Contenido.Any(c => c.CantidadOrdenada <= 0 || c.PrecioUnitario < 0))
                return BadRequest("Las cantidades y precios no son válidos.");

            var ids = pedidoEntrante.Contenido.Select(c => c.ComponenteId).Distinct().ToList();

            var idsValidos = await _context.ProveedoresComponentes
                .Where(pc => pc.IdProveedor == pedidoEntrante.IdProveedor && pc.Activo == true && ids.Contains(pc.IdComponente))
                .Select(pc => pc.IdComponente)
                .ToListAsync();

            if (ids.Except(idsValidos).Any())
                return BadRequest("Uno o más componentes no pertenecen al proveedor.");
          
            
                await using var transaction = await _context.Database.BeginTransactionAsync();

                var registro = new OrdenesCompra
                {
                    IdProveedor = pedidoEntrante.IdProveedor,
                    FolioOrden = $"TEMP-{Guid.NewGuid():N}",
                    Estado = "Pendiente"
                };

                _context.OrdenesCompras.Add(registro);
                await _context.SaveChangesAsync();

                registro.FolioOrden = $"ORD-{DateTime.UtcNow.Year}-NOG-{pedidoEntrante.IdProveedor}-{registro.IdOrdenCompra}";

                foreach (var item in pedidoEntrante.Contenido)
                {
                    _context.OrdenesCompraDetalles.Add(new OrdenesCompraDetalle
                    {
                        IdOrdenCompra = registro.IdOrdenCompra,
                        IdComponente = item.ComponenteId,
                        CantidadOrdenada = item.CantidadOrdenada,
                        PrecioUnitario = item.PrecioUnitario
                    });
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    registro.IdOrdenCompra,
                    registro.FolioOrden
                });
            
            
        }
    }
}
