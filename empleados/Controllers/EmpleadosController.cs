using empleados.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace empleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly EmpleadosDbContext _context;
        private readonly string _connectionString;

        public EmpleadosController(EmpleadosDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleados>>> GetEmpleados()
        {
            return await _context.Empleados.FromSqlRaw("EXEC sp_GetEmpleados").ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Empleados>> GetEmpleado(int id)
        {
            var empleado = await _context.Empleados.FromSqlRaw("EXEC sp_GetEmpleadoById @Id", new SqlParameter("@Id", id)).FirstOrDefaultAsync();
            return empleado ?? (ActionResult<Empleados>)NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Empleados>> CreateEmpleado(Empleados empleado)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC sp_InsertEmpleado @Nombre, @Correo, @FechaNacimiento",
                new SqlParameter("@Nombre", empleado.Nombre),
                new SqlParameter("@Correo", empleado.Correo),
                new SqlParameter("@FechaNacimiento", empleado.FechaNacimiento));
            return CreatedAtAction(nameof(GetEmpleado), new { id = empleado.Id }, empleado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmpleado(int id, Empleados empleado)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC sp_UpdateEmpleado @Id, @Nombre, @Correo, @FechaNacimiento",
                new SqlParameter("@Id", id),
                new SqlParameter("@Nombre", empleado.Nombre),
                new SqlParameter("@Correo", empleado.Correo),
                new SqlParameter("@FechaNacimiento", empleado.FechaNacimiento));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC sp_DeleteEmpleado @Id", new SqlParameter("@Id", id));
            return NoContent();
        }
    }
}
