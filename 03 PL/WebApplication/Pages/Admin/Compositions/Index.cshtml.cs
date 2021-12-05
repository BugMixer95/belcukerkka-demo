using System.Collections.Generic;
using System.Linq;
using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Admin.Compositions
{
    public class IndexModel : PageModel
    {
        private readonly IEntityRepository<Composition> _compositionRepository;

        public IndexModel(IEntityRepository<Composition> compositionRepository)
        {
            _compositionRepository = compositionRepository;
        }

        public IEnumerable<Composition> Compositions { get; set; }

        public IActionResult OnGet()
        {
            Compositions = _compositionRepository.GetAllWithDependencies()
                .OrderBy(c => c.Weight)
                .ThenBy(c => c.WeightType.Name);

            return Page();
        }
    }
}
