using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CLOUD.Models; // Ajusta el namespace si es necesario
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CLOUD.Pages
{
    public class PrestamosModel : PageModel
    {
        private readonly BibliotecaContext _context;
        public PrestamosModel(BibliotecaContext context)
        {
            _context = context;
        }

        public List<Prestamo> Prestamos { get; set; }

        public async Task OnGetAsync()
        {
            Prestamos = await _context.Prestamos
                .Include(p => p.RunUsuarioNavigation) // Cambiar "Usuario" por "RunUsuarioNavigation"  
                .Include(p => p.IdLibroNavigation)    // Cambiar "Libro" por "IdLibroNavigation"  
                .ToListAsync();
        }
    }
}
