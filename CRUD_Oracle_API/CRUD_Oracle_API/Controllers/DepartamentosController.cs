// Controllers/DepartamentosController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD_Oracle_API.Data;
using CRUD_Oracle_API.Models;
using CRUD_Oracle_API.DTOs;
using Oracle.ManagedDataAccess.Client;

namespace CRUD_Oracle_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentosController : ControllerBase
    {
        private readonly OracleDbContext _context;

        public DepartamentosController(OracleDbContext context)
        {
            _context = context;
        }

        // GET: api/departamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartamentoResponseDto>>> GetDepartamentos()
        {
            var departamentos = await _context.Departamentos
                .Select(d => new DepartamentoResponseDto
                {
                    IdDepartamento = d.IdDepartamento,
                    NombreDepartamento = d.NombreDepartamento
                })
                .ToListAsync();

            return Ok(departamentos);
        }

        // GET: api/departamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartamentoResponseDto>> GetDepartamento(int id)
        {
            var departamento = await _context.Departamentos
                .Where(d => d.IdDepartamento == id)
                .Select(d => new DepartamentoResponseDto
                {
                    IdDepartamento = d.IdDepartamento,
                    NombreDepartamento = d.NombreDepartamento
                })
                .FirstOrDefaultAsync();

            if (departamento == null)
                return NotFound(new { message = "Departamento no encontrado." });

            return Ok(departamento);
        }

        // POST: api/departamentos
        [HttpPost]
        public async Task<ActionResult<DepartamentoResponseDto>> CreateDepartamento(DepartamentoRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NombreDepartamento))
                return BadRequest(new { message = "El nombre del departamento es obligatorio." });

            var departamento = new Departamento
            {
                NombreDepartamento = dto.NombreDepartamento.Trim()
            };

            try
            {
                _context.Departamentos.Add(departamento);
                await _context.SaveChangesAsync();

                var response = new DepartamentoResponseDto
                {
                    IdDepartamento = departamento.IdDepartamento,
                    NombreDepartamento = departamento.NombreDepartamento
                };

                return CreatedAtAction(nameof(GetDepartamento), new { id = response.IdDepartamento }, response);
            }
            catch (DbUpdateException ex) when (ex.InnerException is OracleException oracleEx)
            {
                var error = MapOracleError(oracleEx);
                return BadRequest(new { message = error });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error interno al crear el departamento." });
            }
        }

        // PUT: api/departamentos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartamento(int id, DepartamentoRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NombreDepartamento))
                return BadRequest(new { message = "El nombre del departamento es obligatorio." });

            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null)
                return NotFound(new { message = "Departamento no encontrado." });

            departamento.NombreDepartamento = dto.NombreDepartamento.Trim();

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex) when (ex.InnerException is OracleException oracleEx)
            {
                var error = MapOracleError(oracleEx);
                return BadRequest(new { message = error });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error interno al actualizar el departamento." });
            }
        }

        // DELETE: api/departamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartamento(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null)
                return NotFound(new { message = "Departamento no encontrado." });

            try
            {
                _context.Departamentos.Remove(departamento);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex) when (ex.InnerException is OracleException oracleEx)
            {
                if (oracleEx.Number == 2292)
                    return BadRequest(new { message = "No se puede eliminar: hay ciudades asociadas a este departamento." });

                var error = MapOracleError(oracleEx);
                return BadRequest(new { message = error });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error interno al eliminar el departamento." });
            }
        }

        // Mapeo de errores Oracle → mensajes amigables
        private static string MapOracleError(OracleException ex)
        {
            return ex.Number switch
            {
                1 => "Ya existe un departamento con este nombre.", // ORA-00001
                1400 => "El nombre del departamento no puede estar vacío.", // ORA-01400
                2292 => "No se puede eliminar: hay ciudades asociadas.", // ORA-02292
                _ => $"Error de base de datos: {ex.Message}"
            };
        }
    }
}