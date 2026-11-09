using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoreoREST.Models;
using MonitoreoREST.Models.Dtos.Componentes;
namespace MonitoreoREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class componentesController : ControllerBase
    {
        private readonly BdMonitoreoContext _context;
        private readonly IWebHostEnvironment _env;

        public componentesController(BdMonitoreoContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetComponentes()
        {
            var componentes = await _context.Componentes
                .AsNoTracking()
                .Where(c => c.Activo == true) // Filtrar solo los componentes activos
                .Select(c => new Consulta
                {
                    IdComponente = c.IdComponente,
                    Codigo = c.Codigo,
                    Nombre = c.Nombre,
                    Marca = c.Marca,
                    Descripcion = c.Descripcion,
                    Categoria = c.IdCategoriaNavigation.Nombre,
                    UnidadMedida = c.UnidadMedida,
                    ImagenUrl = c.ImagenUrl
                })
                .ToListAsync();
            return Ok(componentes);
        }
        [HttpGet("view/{id:int}")]
        public async Task<IActionResult> GetComponente(int id)
        {
            var componente = await _context.Componentes
                .AsNoTracking()
                .Where(c => c.IdComponente == id)
                .Select(c => new Consulta
                {
                    IdComponente = c.IdComponente,
                    Codigo = c.Codigo,
                    Nombre = c.Nombre,
                    Marca = c.Marca,
                    Descripcion = c.Descripcion,
                    Categoria = c.IdCategoriaNavigation.Nombre,
                    UnidadMedida = c.UnidadMedida,
                    StockMaximo = c.StockMaximo,
                    StockMinimo = c.StockMinimo,
                    ImagenUrl = c.ImagenUrl
                })
                .FirstOrDefaultAsync();
            if (componente is null) return NotFound();
            return Ok(componente);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateComponente([FromForm] Crear componente)
        {
            if (await _context.Componentes.AnyAsync(c => c.Codigo == componente.Codigo))
                return Conflict("Ya existe un componente con ese código.");

            if (componente.StockMaximo < componente.StockMinimo)
                return BadRequest("El stock máximo no puede ser menor que el stock mínimo.");

            string? imagenUrl = null;

            if (componente.Imagen is not null)
            {
                imagenUrl = await CrearUrl(componente.Imagen);
                if (imagenUrl is null) return BadRequest("Imagen inválida. Solo JPG, PNG y WebP de máximo 5 MB.");
            }

            var componenteNuevo = new Componente
            {
                Codigo = componente.Codigo,
                Nombre = componente.Nombre,
                Marca = componente.Marca,
                Descripcion = componente.Descripcion,
                IdCategoria = componente.IdCategoria,
                UnidadMedida = componente.UnidadMedida,
                StockMinimo = componente.StockMinimo,
                StockMaximo = componente.StockMaximo,
                ImagenUrl = imagenUrl,
                Activo = true
            };

            _context.Componentes.Add(componenteNuevo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComponente), new { id = componenteNuevo.IdComponente },
                new { id = componenteNuevo.IdComponente, imagenUrl });
        }

        private async Task<string?> CrearUrl(IFormFile imagen)
        {
            string? extension = await ValidarMagicBytes(imagen);
            if (extension is null) return null;

            string carpeta = Path.Combine(_env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot"), "uploads", "componentes");
            Directory.CreateDirectory(carpeta);

            string nombre = $"{Guid.NewGuid():N}{extension}";
            string ruta = Path.Combine(carpeta, nombre);

            using var stream = new FileStream(ruta, FileMode.CreateNew);
            await imagen.CopyToAsync(stream);

            return $"/uploads/componentes/{nombre}";
        }

        private async Task<string?> ValidarMagicBytes(IFormFile imagen)
        {
            if (imagen.Length == 0 || imagen.Length > 5 * 1024 * 1024)
                return null;

            byte[] header = new byte[12];

            using var stream = imagen.OpenReadStream();
            if (await stream.ReadAsync(header, 0, header.Length) < 12)
                return null;

            if (header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF)
                return ".jpg";

            if (header[..8].SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }))
                return ".png";

            if (System.Text.Encoding.ASCII.GetString(header, 0, 4) == "RIFF" &&
                System.Text.Encoding.ASCII.GetString(header, 8, 4) == "WEBP")
                return ".webp";

            return null;
        }
        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> UpdateComponente(int id, [FromForm] Crear componente)
        {
            var existente = await _context.Componentes.FindAsync(id);
            if (existente is null) return NotFound("Componente no encontrado.");

            if (await _context.Componentes.AnyAsync(c => c.Codigo == componente.Codigo && c.IdComponente != id))
                return Conflict("Ya existe un componente con ese código.");

            if (componente.StockMaximo < componente.StockMinimo)
                return BadRequest("El stock máximo no puede ser menor que el mínimo.");
            if (!await _context.Categorias.AnyAsync(c => c.IdCategoria == componente.IdCategoria))
                return BadRequest("La categoría seleccionada no existe.");
            string? imagenAnterior = existente.ImagenUrl;
            string? imagenNueva = null;

            if (componente.Imagen is not null)
            {
                imagenNueva = await CrearUrl(componente.Imagen);
                if (imagenNueva is null) return BadRequest("Imagen inválida.");
            }

            existente.Codigo = componente.Codigo;
            existente.Nombre = componente.Nombre;
            existente.Marca = componente.Marca;
            existente.Descripcion = componente.Descripcion;
            existente.IdCategoria = componente.IdCategoria;
            existente.UnidadMedida = componente.UnidadMedida;
            existente.StockMinimo = componente.StockMinimo;
            existente.StockMaximo = componente.StockMaximo;

            if (imagenNueva is not null)
                existente.ImagenUrl = imagenNueva;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (imagenNueva is not null) EliminarImagen(imagenNueva);
                throw;
            }

            if (imagenNueva is not null && imagenAnterior is not null)
                EliminarImagen(imagenAnterior);

            return NoContent();
        }
        private void EliminarImagen(string imagenUrl)
        {
            const string prefijo = "/uploads/componentes/";

            if (!imagenUrl.StartsWith(prefijo, StringComparison.Ordinal)) return;

            var nombre = imagenUrl[prefijo.Length..];
            if (nombre != Path.GetFileName(nombre)) return;

            var ruta = Path.Combine(
                _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot"),
                "uploads", "componentes", nombre
            );

            if (System.IO.File.Exists(ruta))
                System.IO.File.Delete(ruta);
        }
        [HttpPatch("disable/{id:int}")]
        public async Task<IActionResult> DisableComponente(int id)
        {
            var filas = await _context.Componentes
                .Where(c => c.IdComponente == id)
                .ExecuteUpdateAsync(s => s.SetProperty(c => c.Activo, false));
            if (filas == 0) return NotFound();
            return NoContent();
        }

    }
}
