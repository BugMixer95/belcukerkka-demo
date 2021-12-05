using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages
{
    public class AboutModel : PageModel
    {
        public AboutModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        private readonly IWebHostEnvironment _webHostEnvironment;
        public IActionResult OnGet()
        {
            if (_webHostEnvironment.EnvironmentName == "Staging" && !User.Identity.IsAuthenticated)
                return RedirectToPage("/Maintain");

            return Page();
        }
    }
}
