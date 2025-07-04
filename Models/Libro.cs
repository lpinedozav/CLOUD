using System;
using System.Collections.Generic;

namespace CLOUD.Models;

public partial class Libro
{
    public string IdLibro { get; set; } = null!;

    public string NombreLibro { get; set; } = null!;

    public string? Autor { get; set; }

    public int? Copia { get; set; }

    public bool? Disponible { get; set; }

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
