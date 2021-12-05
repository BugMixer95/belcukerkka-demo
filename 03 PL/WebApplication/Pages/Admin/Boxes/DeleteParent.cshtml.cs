using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Admin.Boxes
{
    public class DeleteParentModel : PageModel
    {
        private readonly IEntityRepository<BoxParent> _boxParentRepository;

        public DeleteParentModel(IEntityRepository<BoxParent> boxParentRepository)
        {
            _boxParentRepository = boxParentRepository;
        }

        [BindProperty]
        public BoxParent BoxParent { get; set; }

        public IActionResult OnGet(int id)
        {
            BoxParent = _boxParentRepository.GetWithDependencies(id);

            if (BoxParent == null)
                return RedirectToPage("Error");

            return Page();
        }

        public IActionResult OnPost()
        {
            BoxParent = _boxParentRepository.Delete(BoxParent.Id);

            return RedirectToPage("Index");
        }
    }
}
