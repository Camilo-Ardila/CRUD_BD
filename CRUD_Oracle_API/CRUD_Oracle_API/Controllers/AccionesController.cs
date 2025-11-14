// Controllers/AccionesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD_Oracle_API.Data;
using CRUD_Oracle_API.Models;

namespace CRUD_Oracle_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccionesController : ControllerBase
    {
        private readonly OracleDbContext _context;

        public AccionesController(OracleDbContext context)
        {
            _context = context;
        }

        // GET: api/Acciones
        [HttpGet]
        public async Task<IActionResult> GetAcciones()
        {
            try
            {
                // Prueba ultra-simple
                var acciones = await _context.Acciones.ToListAsync();
                return Ok(acciones);
            }
            catch (Exception ex)
            {
                // Esto debería aparecer en la consola del servidor
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine($"STACK: {ex.StackTrace}");

                return StatusCode(500, new
                {
                    message = "Error al obtener las acciones",
                    error = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }

        // GET: api/Acciones/{tipoAccion}
        [HttpGet("{tipoAccion}")]
        public async Task<IActionResult> GetAccion(string tipoAccion)
        {
            try
            {
                var accion = await _context.Acciones.FindAsync(tipoAccion);

                if (accion == null)
                {
                    return NotFound(new { message = $"No se encontró la acción '{tipoAccion}'" });
                }

                return Ok(accion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al obtener la acción",
                    error = ex.Message
                });
            }
        }
    }
}