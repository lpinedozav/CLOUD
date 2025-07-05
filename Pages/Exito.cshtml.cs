using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CLOUD.Pages
{
    public class ExitoModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? TipoOperacion { get; set; }
        public void OnGet()
        {
        }
    }
}
