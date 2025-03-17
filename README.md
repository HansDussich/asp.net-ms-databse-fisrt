# empleados

```bash
CREATE DATABASE EmpleadosDB;
GO

USE EmpleadosDB;
GO

CREATE TABLE Empleados (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) UNIQUE NOT NULL,
    FechaNacimiento DATE NOT NULL
);

```

```bash

-- Obtener todos los empleados
CREATE PROCEDURE sp_GetEmpleados
AS
BEGIN
    SELECT * FROM Empleados;
END;
GO

-- Obtener empleado por ID
CREATE PROCEDURE sp_GetEmpleadoById @Id INT
AS
BEGIN
    SELECT * FROM Empleados WHERE Id = @Id;
END;
GO

-- Insertar empleado
CREATE PROCEDURE sp_InsertEmpleado @Nombre NVARCHAR(100), @Correo NVARCHAR(100), @FechaNacimiento DATE
AS
BEGIN
    INSERT INTO Empleados (Nombre, Correo, FechaNacimiento) VALUES (@Nombre, @Correo, @FechaNacimiento);
    SELECT SCOPE_IDENTITY() AS Id;
END;
GO

-- Actualizar empleado
CREATE PROCEDURE sp_UpdateEmpleado @Id INT, @Nombre NVARCHAR(100), @Correo NVARCHAR(100), @FechaNacimiento DATE
AS
BEGIN
    UPDATE Empleados SET Nombre = @Nombre, Correo = @Correo, FechaNacimiento = @FechaNacimiento WHERE Id = @Id;
END;
GO

-- Eliminar empleado
CREATE PROCEDURE sp_DeleteEmpleado @Id INT
AS
BEGIN
    DELETE FROM Empleados WHERE Id = @Id;
END;
GO

```

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Mvc.Versioning
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
```

```bash

dotnet add package Microsoft.EntityFrameworkCore.Design
```
```bash
public class Empleados
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
    }
```


```bash
using Microsoft.EntityFrameworkCore;

namespace MicroservicioEmpleados.Models
{
    public class EmpleadosDbContext : DbContext
    {
        public EmpleadosDbContext(DbContextOptions<EmpleadosDbContext> options) : base(options) { }
        public DbSet<Empleados> Empleados { get; set; }
    }

```

```bash
builder.Services.AddDbContext<EmpleadosDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

```


```bash
dotnet aspnet-codegenerator controller -name EmpleadosController -async -api -m Empleados -dc EmpleadosDbContext -outDir Controllers
```

```bash
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
```

```bash

dotnet aspnet-codegenerator controller -name EmpleadosController -async -api -m Empleados -dc EmpleadosDbContext -outDir Controllers
```


