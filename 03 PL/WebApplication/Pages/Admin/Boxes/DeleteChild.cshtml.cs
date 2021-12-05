using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Admin.Boxes
{
    public class DeleteChildModel : PageModel
    {
        private readonly IEntityRepository<Box> _boxRepository;

        public DeleteChildModel(IEntityRepository<Box> boxRepository)
        {
            _boxRepository = boxRepository;
        }

        [BindProperty]
        public Box Box { get; set; }

        public IActionResult OnGet(int id)
        {
            Box = _boxRepository.Get(id);

            if (Box == null)
                return RedirectToPage("Error");

            return Page();
        }

        public IActionResult OnPost()
        {
            Box = _boxRepository.Delete(Box.Id);

            return RedirectToPage("Index");
        }
    }
}
