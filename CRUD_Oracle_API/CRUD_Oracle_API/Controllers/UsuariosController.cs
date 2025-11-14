// Controllers/UsuariosController.cs
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
    public class UsuariosController : ControllerBase
    {
        private readonly OracleDbContext _context;

        public UsuariosController(OracleDbContext context)
        {
            _context = context;
        }

        // GET: api/usuarios?nombre=juan&fechaDesde=2000-01-01&fechaHasta=2005-12-31
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetUsuarios(
            [FromQuery] string? nombre,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta)
        {
            var query = _context.Usuarios.Include(u => u.Ciudad).AsQueryable();

            // FILTRO POR NOMBRE (búsqueda parcial, case-insensitive)
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var nombreLower = nombre.ToLower();
                query = query.Where(u => u.Nombre.ToLower().Contains(nombreLower));
            }

            // FILTRO POR RANGO DE FECHAS DE NACIMIENTO
            if (fechaDesde.HasValue)
            {
                query = query.Where(u => u.FechaNacimiento >= fechaDesde.Value.Date);
            }

            if (fechaHasta.HasValue)
            {
                query = query.Where(u => u.FechaNacimiento <= fechaHasta.Value.Date);
            }

            var usuarios = await query
                .Select(u => new
                {
                    u.Cc,
                    u.Nombre,
                    u.FechaNacimiento,
                    u.IdCiudad,
                    NombreCiudad = u.Ciudad != null ? u.Ciudad.NombreCiudad : "Desconocida",
                    IdDepartamento = u.Ciudad != null ? u.Ciudad.IdDepartamento : 0
                })
                .ToListAsync();

            var deptIds = usuarios
                .Where(x => x.IdDepartamento > 0)
                .Select(x => x.IdDepartamento)
                .Distinct()
                .ToList();

            var departamentos = await _context.Departamentos
                .Where(d => deptIds.Contains(d.IdDepartamento))
                .ToDictionaryAsync(d => d.IdDepartamento, d => d.NombreDepartamento);

            var response = usuarios.Select(u => new UsuarioResponseDto
            {
                Cc = u.Cc,
                Nombre = u.Nombre,
                FechaNacimiento = u.FechaNacimiento,
                IdCiudad = u.IdCiudad,
                NombreCiudad = u.NombreCiudad,
                NombreDepartamento = u.IdDepartamento > 0 && departamentos.ContainsKey(u.IdDepartamento)
                    ? departamentos[u.IdDepartamento]
                    : "Desconocido"
            }).ToList();

            return Ok(response);
        }

        // GET: api/usuarios/1234567890
        [HttpGet("{cc}")]
        public async Task<ActionResult<UsuarioResponseDto>> GetUsuario(long cc)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Ciudad)
                .Where(u => u.Cc == cc)
                .Select(u => new
                {
                    u.Cc,
                    u.Nombre,
                    u.FechaNacimiento,
                    u.IdCiudad,
                    NombreCiudad = u.Ciudad != null ? u.Ciudad.NombreCiudad : "Desconocida",
                    IdDepartamento = u.Ciudad != null ? u.Ciudad.IdDepartamento : 0
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound(new { message = "Usuario no encontrado." });

            string nombreDepartamento = "Desconocido";
            if (usuario.IdDepartamento > 0)
            {
                var dept = await _context.Departamentos
                    .Where(d => d.IdDepartamento == usuario.IdDepartamento)
                    .Select(d => d.NombreDepartamento)
                    .FirstOrDefaultAsync();
                nombreDepartamento = dept ?? "Desconocido";
            }

            return Ok(new UsuarioResponseDto
            {
                Cc = usuario.Cc,
                Nombre = usuario.Nombre,
                FechaNacimiento = usuario.FechaNacimiento,
                IdCiudad = usuario.IdCiudad,
                NombreCiudad = usuario.NombreCiudad,
                NombreDepartamento = nombreDepartamento
            });
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDto>> CreateUsuario(UsuarioRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ciudadCount = await _context.Ciudades
                .CountAsync(c => c.IdCiudad == dto.IdCiudad);

            if (ciudadCount == 0)
                return BadRequest(new { message = "La ciudad seleccionada no existe." });

            var usuario = new Usuario
            {
                Cc = dto.Cc,
                Nombre = dto.Nombre.Trim(),
                FechaNacimiento = dto.FechaNacimiento.Date,
                IdCiudad = dto.IdCiudad
            };

            try
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                var ciudad = await _context.Ciudades
                    .Where(c => c.IdCiudad == usuario.IdCiudad)
                    .Select(c => new { c.NombreCiudad, c.IdDepartamento })
                    .FirstOrDefaultAsync();

                if (ciudad == null)
                    return StatusCode(500, new { message = "Ciudad no encontrada después de crear." });

                var departamento = await _context.Departamentos
                    .Where(d => d.IdDepartamento == ciudad.IdDepartamento)
                    .Select(d => d.NombreDepartamento)
                    .FirstOrDefaultAsync();

                var response = new UsuarioResponseDto
                {
                    Cc = usuario.Cc,
                    Nombre = usuario.Nombre,
                    FechaNacimiento = usuario.FechaNacimiento,
                    IdCiudad = usuario.IdCiudad,
                    NombreCiudad = ciudad.NombreCiudad,
                    NombreDepartamento = departamento ?? "Desconocido"
                };

                return CreatedAtAction(nameof(GetUsuario), new { cc = response.Cc }, response);
            }
            catch (DbUpdateException ex) when (ex.InnerException is OracleException oracleEx)
            {
                var error = MapOracleError(oracleEx);
                return BadRequest(new { message = error });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno al crear el usuario: " + ex.Message });
            }
        }

        // PUT: api/usuarios/1234567890
        [HttpPut("{cc}")]
        public async Task<IActionResult> UpdateUsuario(long cc, UsuarioRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (cc != dto.Cc)
                return BadRequest(new { message = "La cédula en la URL debe coincidir con el cuerpo." });

            var usuario = await _context.Usuarios.FindAsync(cc);
            if (usuario == null)
                return NotFound(new { message = "Usuario no encontrado." });

            var ciudadCount = await _context.Ciudades
                .CountAsync(c => c.IdCiudad == dto.IdCiudad);

            if (ciudadCount == 0)
                return BadRequest(new { message = "La ciudad seleccionada no existe." });

            usuario.Nombre = dto.Nombre.Trim();
            usuario.FechaNacimiento = dto.FechaNacimiento.Date;
            usuario.IdCiudad = dto.IdCiudad;

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
                return StatusCode(500, new { message = "Error interno al actualizar el usuario." });
            }
        }

        // DELETE: api/usuarios/1234567890
        [HttpDelete("{cc}")]
        public async Task<IActionResult> DeleteUsuario(long cc)
        {
            var usuario = await _context.Usuarios.FindAsync(cc);
            if (usuario == null)
                return NotFound(new { message = "Usuario no encontrado." });

            try
            {
                _context.Usuarios.Remove(usuario);
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
                return StatusCode(500, new { message = "Error interno al eliminar el usuario." });
            }
        }

        private static string MapOracleError(OracleException ex)
        {
            return ex.Number switch
            {
                1 => "Ya existe un usuario con esta cédula.",
                2291 => "La ciudad seleccionada no existe.",
                20001 => "El usuario debe tener al menos 18 años.",
                2290 => "La cédula debe tener 8 o 10 dígitos.",
                1400 => "Todos los campos son obligatorios.",
                _ => $"Error de base de datos: {ex.Message}"
            };
        }
    }
}