using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD_Oracle_API.Models;
using CRUD_Oracle_API.Data;

namespace CRUD_Oracle_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly OracleDbContext _context;

        public LogsController(OracleDbContext context)
        {
            _context = context;
        }

        // GET: api/logs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Log>>> GetLogs()
        {
            try
            {
                var logs = await _context.Logs
                    .OrderByDescending(l => l.FechaIngreso)
                    .ToListAsync();

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener los logs: " + ex.Message);
            }
        }
    }
}