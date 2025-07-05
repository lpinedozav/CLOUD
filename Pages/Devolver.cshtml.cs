using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CLOUD.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CLOUD.Pages
{
    public class DevolverModel : PageModel
    {
        private readonly BibliotecaContext _context;
        public DevolverModel(BibliotecaContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string IdLibro { get; set; }

        public bool MostrarResultado { get; set; } = false;
        public bool PrestamoEncontrado { get; set; } = false;
        public string Mensaje { get; set; }

        // Info del préstamo
        public string LibroNombre { get; set; }
        public string UsuarioNombre { get; set; }
        public DateOnly? FechaPrestamo { get; set; }
        public DateOnly? FechaDevolucion { get; set; }

        public Prestamo? Prestamo { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var accion = Request.Form["accion"];

            if (accion == "Verificar préstamo")
            {
                MostrarResultado = true;

                Prestamo = await _context.Prestamos.Include(p => p.IdLibroNavigation).Include(p => p.RunUsuarioNavigation)
                    .Where(p => p.IdLibro! == IdLibro && p.Devuelto! == false)
                    .OrderByDescending(p => p.FechaPrestamo)
                    .FirstOrDefaultAsync();

                if (Prestamo == null)
                {
                    PrestamoEncontrado = false;
                    Mensaje = "No existe un préstamo activo para este libro.";
                    return Page();
                }

                PrestamoEncontrado = true;
                LibroNombre = Prestamo.IdLibroNavigation?.NombreLibro!;
                UsuarioNombre = Prestamo.RunUsuarioNavigation?.Nombre!;
                FechaPrestamo = Prestamo.FechaPrestamo;
                FechaDevolucion = Prestamo.FechaDevolucion;
                return Page();
            }
            else if (accion == "Confirmar devolución")
            {
                var prestamo = _context.Prestamos
                    .Where(p => p.IdLibro! == IdLibro && p.Devuelto! == false)
                    .OrderByDescending(p => p.FechaPrestamo)
                    .FirstOrDefault();

                if (prestamo != null)
                {
                    prestamo.Devuelto = true;
                    var libro = _context.Libros.FirstOrDefault(l => l.IdLibro == IdLibro);
                    if (libro != null) libro.Disponible = true;
                    await _context.SaveChangesAsync();
                }

                return RedirectToPage("/Exito", new { TipoOperacion = "devolucion" });
            }

            // Fallback, vuelve a la página inicial si nada coincide
            return Page();
        }
    }
}
