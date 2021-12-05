using System.Collections.Generic;
using System.Threading.Tasks;
using Belcukerkka.Services.Extras;
using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Belcukerkka.Services.Operations;

namespace WebApplication.Pages.Admin.Compositions
{
    public class EditCandyModel : PageModel
    {
        private readonly IEntityRepository<Candy> _candyRepository;
        private readonly IManyToManyService<CandyInComposition> _operationService;

        public EditCandyModel(IEntityRepository<Candy> candyRepository,
            IManyToManyService<CandyInComposition> operationService)
        {
            _candyRepository = candyRepository;
            _operationService = operationService;
        }

        [BindProperty]
        public CandyInComposition CandyInComposition { get; set; }

        public IEnumerable<SelectListItem> CandyNames { get; set; }

        public IActionResult OnGet(int compositionId, int? candyId)
        {
            CandyNames = DropdownHandler.GetSelectList(_candyRepository, "Name");

            if (candyId.HasValue)
                CandyInComposition = _operationService.FindOnGet(compositionId, (int)candyId);
            else
                CandyInComposition = _operationService.CreateOnGet(compositionId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var isEditOperation = _operationService.CheckOperationType(CandyInComposition.CompositionId, CandyInComposition.CandyId);

            if (isEditOperation)
                CandyInComposition = await _operationService.UpdateOnPostAsync(CandyInComposition);
            else
                CandyInComposition = await _operationService.CreateOnPostAsync(CandyInComposition);

            return RedirectToPage("/Admin/Compositions/Edit", new { id = CandyInComposition.CompositionId });
        }
    }
}
