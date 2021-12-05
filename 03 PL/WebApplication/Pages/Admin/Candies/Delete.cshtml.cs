using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Admin.Candies
{
    public class DeleteModel : PageModel
    {
        private readonly IEntityRepository<Candy> _candyRepository;

        public DeleteModel(IEntityRepository<Candy> candyRepository)
        {
            _candyRepository = candyRepository;
        }

        [BindProperty]
        public Candy Candy { get; set; }

        public IActionResult OnGet(int? id)
        {
            Candy = _candyRepository.Get(id);

            if (Candy == null)
                return RedirectToPage("Error");

            return Page();
        }

        public IActionResult OnPost()
        {
            Candy candyToDelete = _candyRepository.Delete(Candy.Id);

            if (candyToDelete == null)
                return RedirectToPage("Error");

            return RedirectToPage("Index");
        }
    }
}
