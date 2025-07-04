using System;
using System.Collections.Generic;

namespace CLOUD.Models;

public partial class Usuario
{
    public string Run { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public DateOnly? FechaNacimiento { get; set; }

    public string? Direccion { get; set; }

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
