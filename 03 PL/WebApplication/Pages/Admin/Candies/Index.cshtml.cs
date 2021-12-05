using System.Collections.Generic;
using System.Linq;
using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Admin.Candies
{
    public class IndexModel : PageModel
    {
        private readonly IEntityRepository<Candy> _candyRepository;

        public IndexModel(IEntityRepository<Candy> candyRepository)
        {
            _candyRepository = candyRepository;
        }

        public IEnumerable<Candy> Candies { get; set; }

        public void OnGet()
        {
            Candies = _candyRepository.GetAll().OrderBy(c => c.Name);
        }
    }
}
