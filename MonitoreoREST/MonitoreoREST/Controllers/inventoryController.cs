using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonitoreoREST.Models.Dtos.Inventory;
namespace MonitoreoREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class inventoryController : ControllerBase
    {
        [HttpPost("validar/{id:int}")]
        public async Task<IActionResult> ValidateInventory(int id, [FromBody] CantidadesReales cantidades)
        {
            // Implementation for validating inventory for a specific component
            return Ok("Inventory validated.");
        }
    }
}
          
