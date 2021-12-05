using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages
{
    public class MaintainModel : PageModel
    {
        public MaintainModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        private readonly IWebHostEnvironment _webHostEnvironment;

        public IActionResult OnGet()
        {
            if (_webHostEnvironment.EnvironmentName != "Staging")
                return RedirectToPage("/Index");

            return Page();
        }
    }
}
