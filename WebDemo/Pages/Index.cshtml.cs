using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TextConverterLibrary;

namespace WebDemo.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Format InputFormat { get; set; }
        [BindProperty]
        public string Input { get; set; }
        [BindProperty]
        public string Output { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            InputFormat = Format.XML;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            return RedirectToPage("/prezi");
        }
    }
}