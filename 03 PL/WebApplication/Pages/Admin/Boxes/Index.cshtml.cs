using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Admin.Boxes
{
    public class IndexModel : PageModel
    {
        
        private readonly IEntityRepository<BoxParent> _boxParentRepository;

        public IndexModel(IEntityRepository<BoxParent> boxParentRepository)
        {
            _boxParentRepository = boxParentRepository;
        }

        public IEnumerable<BoxParent> BoxParents { get; set; }

        public IActionResult OnGet()
        {
            BoxParents = _boxParentRepository.GetAllWithDependencies();

            return Page();
        }
    }
}
