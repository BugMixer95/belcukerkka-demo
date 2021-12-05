using System.Collections.Generic;
using System.Linq;
using Belcukerkka.Services.Extras;
using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication.Pages.Admin.Compositions
{
    public class EditModel : PageModel
    {
        private readonly IEntityRepository<Composition> _compositionRepository;
        private readonly IEntityRepository<WeightType> _weightTypeRepository;
        private readonly IEntityRepository<Candy> _candyRepository;

        public EditModel(IEntityRepository<Composition> compositionRepository,
            IEntityRepository<WeightType> weightTypeRepository,
            IEntityRepository<Candy> candyRepository)
        {
            _compositionRepository = compositionRepository;
            _weightTypeRepository = weightTypeRepository;
            _candyRepository = candyRepository;

            WeightTypeNames = DropdownHandler.GetSelectList(_weightTypeRepository, "Name");
            CandyNames = DropdownHandler.GetSelectList(_candyRepository, "Name");
        }

        [BindProperty]
        public Composition Composition { get; set; }

        public IEnumerable<SelectListItem> WeightTypeNames { get; set; }
        public IEnumerable<SelectListItem> CandyNames { get; set; }

        [TempData]
        public string Notification { get; set; }

        public IActionResult OnGet(int? id)
        {
            Notification = string.Empty;

            if (id.HasValue)
                Composition = _compositionRepository.GetWithDependencies((int)id);
            else
                Composition = new Composition();

            if (Composition == null)
                return RedirectToPage("/Admin/Compositions/Index");

            return Page();
        }

        public IActionResult OnPostSaveComposition()
        {
            if (!ModelState.IsValid)
                return Page();

            var existingComposition = _compositionRepository.GetAll()
                .Where(c => c.Weight == Composition.Weight && c.WeightTypeId == Composition.WeightTypeId)
                .FirstOrDefault();

            if (existingComposition != null && existingComposition.Id != Composition.Id)
            {
                Notification = "Такой состав уже существует. Пожалуйста, выберите другие параметры.";
                return Page();
            }

            if (Composition.Id > 0)
                Composition = _compositionRepository.Update(Composition);
            else
                Composition = _compositionRepository.Create(Composition);

            return RedirectToPage("Index");
        }
    }
}
