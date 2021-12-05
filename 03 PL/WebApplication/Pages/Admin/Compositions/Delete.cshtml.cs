using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Admin.Compositions
{
    public class DeleteModel : PageModel
    {
        private readonly IEntityRepository<Composition> _compositionRepository;

        public DeleteModel(IEntityRepository<Composition> compositionRepository)
        {
            _compositionRepository = compositionRepository;
        }

        [BindProperty]
        public Composition Composition { get; set; }

        public IActionResult OnGet(int? id)
        {
            Composition = _compositionRepository.Get(id);

            if (Composition == null)
                return RedirectToPage("Error");

            return Page();
        }

        public IActionResult OnPost()
        {
            Composition compositionToDelete = _compositionRepository.Delete(Composition.Id);

            if (compositionToDelete == null)
                return RedirectToPage("Error");

            return RedirectToPage("Index");
        }
    }
}
