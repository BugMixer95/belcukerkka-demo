using Belcukerkka.Models.Entities;
using Belcukerkka.Models.ViewModels;
using Belcukerkka.Repositories.Interfaces;
using Belcukerkka.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Catalog
{
    public class ItemModel : PageModel
    {
        private readonly IEntityRepository<Box> _boxRepository;
        private readonly ICatalogItemRepository _catalogitemRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ItemModel(IEntityRepository<Box> boxRepository,
            ICatalogItemRepository catalogitemRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _boxRepository = boxRepository;
            _catalogitemRepository = catalogitemRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public Box Box { get; set; }

        [BindProperty]
        public int Amount { get; set; }

        public CatalogItemViewModel CatalogItemModel { get; set; }

        public IActionResult OnGet(string box, int weight, int boxId, string composition)
        {
            if (_webHostEnvironment.EnvironmentName == "Staging" && !User.Identity.IsAuthenticated)
                return RedirectToPage("/Maintain");

            Box = _boxRepository.GetWithDependencies(boxId);

            var catalogItem = _catalogitemRepository.Get(Box.BoxParentId, Box.Composition.Weight);
            CatalogItemModel = CatalogItemConverter.ConvertSingleCatalogItem(catalogItem);

            return Page();
        }
    }
}
