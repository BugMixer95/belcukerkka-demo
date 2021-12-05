using System.Collections.Generic;
using System.IO;
using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Belcukerkka.Services.Extras;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication.Pages.Admin.Boxes
{
    public class EditChildModel : PageModel
    {
        private readonly IEntityRepository<Box> _boxRepository;
        private readonly IEntityRepository<BoxParent> _boxParentRepository;
        private readonly IEntityRepository<Composition> _compositionRepository;

        public EditChildModel(IEntityRepository<Box> boxRepository,
            IEntityRepository<BoxParent> boxParentRepository,
            IEntityRepository<Composition> compositionRepository)
        {
            _boxRepository = boxRepository;
            _boxParentRepository = boxParentRepository;
            _compositionRepository = compositionRepository;
        }

        [BindProperty]
        public Box Box { get; set; }

        public string BoxParentName { get; set; }

        public IEnumerable<SelectListItem> WeightValues { get; set; }
        public IEnumerable<SelectListItem> WeightTypesNames { get; set; }

        public IActionResult OnGet(int boxParentId, int? id)
        {
            WeightValues = CompositionDropdownHandler.GetDistinctedWeightsSelectList(_compositionRepository);

            BoxParentName = _boxParentRepository.Get(boxParentId).Name;

            if (id.HasValue)
                Box = _boxRepository.GetWithDependencies((int)id);
            else
                Box = new Box() { BoxParentId = boxParentId };

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Box.Id > 0)
                Box = _boxRepository.Update(Box);
            else
                Box = _boxRepository.Create(Box);

            return RedirectToPage("Index");
        }

        public IActionResult OnPostGetWeightTypeNames()
        {
            MemoryStream stream = new MemoryStream();
            Request.Body.CopyToAsync(stream);
            stream.Position = 0;

            using (StreamReader reader = new StreamReader(stream))
            {
                string requestBody = reader.ReadToEnd();

                if (requestBody.Length > 0)
                {
                    IEnumerable<SelectListItem> weightTypeNames = CompositionDropdownHandler
                        .GetWeightTypeNames(_boxRepository, _compositionRepository, requestBody);

                    return new JsonResult(weightTypeNames);
                }
            }

            return null;
        }
    }
}
