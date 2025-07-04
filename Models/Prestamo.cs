using System;
using System.Collections.Generic;

namespace CLOUD.Models;

public partial class Prestamo
{
    public int IdPrestamo { get; set; }

    public string? IdLibro { get; set; }

    public string? RunUsuario { get; set; }

    public DateOnly FechaPrestamo { get; set; }

    public DateOnly FechaDevolucion { get; set; }

    public bool? Devuelto { get; set; }

    public virtual Libro? IdLibroNavigation { get; set; }

    public virtual Usuario? RunUsuarioNavigation { get; set; }
}
