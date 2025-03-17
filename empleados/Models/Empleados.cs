using System;
using System.Collections.Generic;

namespace empleados.Models;

public partial class Empleados
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public DateOnly? FechaNacimiento { get; set; }
}
