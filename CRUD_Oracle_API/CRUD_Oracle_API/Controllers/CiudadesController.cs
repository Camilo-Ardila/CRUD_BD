// Controllers/CiudadesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD_Oracle_API.Data;
using CRUD_Oracle_API.Models;

namespace CRUD_Oracle_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CiudadesController : ControllerBase
    {
        private readonly OracleDbContext _context;

        public CiudadesController(OracleDbContext context)
        {
            _context = context;
        }

        // GET: api/ciudades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCiudades()
        {
            var ciudades = await _context.Ciudades
                .Include(c => c.Departamento) // NECESARIO
                .OrderBy(c => c.IdCiudad)
                .Select(c => new
                {
                    c.IdCiudad,
                    c.NombreCiudad,
                    IdDepartamento = c.IdDepartamento,
                    Departamento = c.Departamento.NombreDepartamento // CLAVE
                })
                .ToListAsync();

            return Ok(ciudades);
        }

        // GET: api/ciudades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCiudad(int id)
        {
            var ciudad = await _context.Ciudades
                .Include(c => c.Departamento)
                .Where(c => c.IdCiudad == id)
                .Select(c => new
                {
                    c.IdCiudad,
                    c.NombreCiudad,
                    IdDepartamento = c.IdDepartamento,
                    Departamento = c.Departamento.NombreDepartamento
                })
                .FirstOrDefaultAsync();

            if (ciudad == null)
                return NotFound();

            return Ok(ciudad);
        }

        // POST: api/ciudades
        [HttpPost]
        public async Task<ActionResult> CreateCiudad(CiudadRequestDto dto)
        {
            var count = await _context.Departamentos
                .CountAsync(d => d.IdDepartamento == dto.IdDepartamento);

            if (count == 0)
                return BadRequest("El departamento no existe.");

            var ciudad = new Ciudad
            {
                NombreCiudad = dto.NombreCiudad,
                IdDepartamento = dto.IdDepartamento
            };

            _context.Ciudades.Add(ciudad);
            await _context.SaveChangesAsync();

            // Cargar nombre del departamento
            var result = new
            {
                ciudad.IdCiudad,
                ciudad.NombreCiudad,
                IdDepartamento = ciudad.IdDepartamento,
                Departamento = await _context.Departamentos
                    .Where(d => d.IdDepartamento == ciudad.IdDepartamento)
                    .Select(d => d.NombreDepartamento)
                    .FirstOrDefaultAsync()
            };

            return CreatedAtAction(nameof(GetCiudad), new { id = ciudad.IdCiudad }, result);
        }

        // PUT: api/ciudades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCiudad(int id, CiudadRequestDto dto)
        {
            // VALIDAR ID
            if (id != dto.IdCiudad)
                return BadRequest("El ID en la URL y el cuerpo deben coincidir.");

            // BUSCAR CIUDAD
            var ciudad = await _context.Ciudades.FindAsync(id);
            if (ciudad == null)
                return NotFound("Ciudad no encontrada.");

            // VALIDAR DEPARTAMENTO
            var count = await _context.Departamentos
                .CountAsync(d => d.IdDepartamento == dto.IdDepartamento);

            if (count == 0)
                return BadRequest("El departamento no existe.");

            // ACTUALIZAR
            ciudad.NombreCiudad = dto.NombreCiudad;
            ciudad.IdDepartamento = dto.IdDepartamento;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al actualizar ciudad.",
                    error = ex.InnerException?.Message
                });
            }
        }

        // DELETE: api/ciudades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCiudad(int id)
        {
            var ciudad = await _context.Ciudades.FindAsync(id);
            if (ciudad == null)
                return NotFound();

            _context.Ciudades.Remove(ciudad);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    // DTO
    public class CiudadRequestDto
    {
        public int IdCiudad { get; set; } // OBLIGATORIO EN PUT
        public string NombreCiudad { get; set; } = null!;
        public int IdDepartamento { get; set; }
    }
}