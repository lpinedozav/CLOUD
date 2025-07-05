using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CLOUD.Models; // Ajusta el namespace de tus modelos
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CLOUD.Pages
{
    public class SolicitarModel : PageModel
    {
        private readonly BibliotecaContext _context;
        public SolicitarModel(BibliotecaContext context)
        {
            _context = context;
        }

        [BindProperty] public string Run { get; set; }
        [BindProperty] public string NombreUsuario { get; set; }
        [BindProperty] public string IdLibro { get; set; }
        [BindProperty] public DateTime FechaPrestamo { get; set; }
        [BindProperty] public DateTime FechaDevolucion { get; set; }

        public List<Libro> LibrosDisponibles { get; set; }

        public async Task OnGetAsync() // Cambiar a Task para manejar await correctamente
        {
            LibrosDisponibles = await _context.Libros
                .Where(l => l.Disponible == true) // Manejar el tipo nullable correctamente
                .ToListAsync(); // Usar ToListAsync para trabajar con EF Core

            FechaPrestamo = DateTime.Today;          // 2025-07-05, por ejemplo
            FechaDevolucion = DateTime.Today.AddDays(14); // 2 semanas, si quieres
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Buscar el usuario por RUN
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Run == Run);

            if (usuario == null)
            {
                // Si no existe, crearlo (puedes ajustar para pedir más datos si necesitas)
                usuario = new Usuario
                {
                    Run = Run,
                    Nombre = NombreUsuario ?? "(Sin nombre)",
                    FechaNacimiento = null, // O puedes pedir este dato en el formulario
                    Direccion = "" // O puedes pedir este dato en el formulario
                };
                await _context.Usuarios.AddAsync(usuario);
            }

            NombreUsuario = usuario.Nombre;

            var prestamo = new Prestamo
            {
                IdLibro = IdLibro,
                RunUsuario = Run,
                FechaPrestamo = DateOnly.FromDateTime(FechaPrestamo),
                FechaDevolucion = DateOnly.FromDateTime(FechaDevolucion),
                Devuelto = false
            };

            await _context.Prestamos.AddAsync(prestamo);

            // Marcar libro como no disponible
            var libro = await _context.Libros.FirstOrDefaultAsync(l => l.IdLibro == IdLibro);
            if (libro != null)
            {
                libro.Disponible = false;
            }
            await _context.SaveChangesAsync();
            return RedirectToPage("/Exito", new { TipoOperacion = "prestamo" });
        }

    }
}
