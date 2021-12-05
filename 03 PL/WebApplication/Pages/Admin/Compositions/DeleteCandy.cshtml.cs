using Belcukerkka.Models.Entities;
using Belcukerkka.Services.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace WebApplication.Pages.Admin.Compositions
{
    public class DeleteCandyModel : PageModel
    {
        private readonly IManyToManyService<CandyInComposition> _operationService;

        public DeleteCandyModel(IManyToManyService<CandyInComposition> operationService)
        {
            _operationService = operationService;
        }

        [BindProperty(SupportsGet = true)]
        public int CandyId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CompositionId { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var candyInComposition = await _operationService.DeleteOnPostAsync(CompositionId, CandyId);

            return RedirectToPage("./Edit", new { id = CompositionId });
        }
    }
}
