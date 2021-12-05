using Belcukerkka.Models.Entities;
using Belcukerkka.Models.ViewModels;
using Belcukerkka.Repositories.Interfaces;
using Belcukerkka.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel(ILogger<IndexModel> logger,
            IWebHostEnvironment webHostEnvironment,
            ICatalogItemRepository catalogItemRepository,
            IPartialToStringRenderer partialToStringRenderer,
            IEntityRepository<Composition> compositionRepository,
            IEntityRepository<Box> boxRepository)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _catalogItemRepository = catalogItemRepository;
            _partialToStringRenderer = partialToStringRenderer;
            _compositionRepository = compositionRepository;
            _boxRepository = boxRepository;
        }

        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICatalogItemRepository _catalogItemRepository;
        private readonly IPartialToStringRenderer _partialToStringRenderer;
        private readonly IEntityRepository<Composition> _compositionRepository;
        private readonly IEntityRepository<Box> _boxRepository;

        public IEnumerable<CatalogItemViewModel> CatalogItems { get; set; }

        public IEnumerable<int> AvailableWeights { get; set; }

        public IActionResult OnGet()
        {
            if (_webHostEnvironment.EnvironmentName == "Staging" && !User.Identity.IsAuthenticated)
                return RedirectToPage("/Maintain");

            var defaultWeight = 1000;

            CatalogItems = new List<CatalogItemViewModel>();
            var catalogItems = _catalogItemRepository.GetAll()
                .Where(ci => ci.Weight == defaultWeight)
                .Take(4);

            using (var handler = new CatalogFiltersHandler(_compositionRepository, _boxRepository))
            {
                AvailableWeights = handler.GetAvailableWeights()
                    .OrderBy(w => w)
                    .ToList();
            }

            foreach (var item in catalogItems)
            {
                var model = CatalogItemConverter.ConvertSingleCatalogItem(item);

                CatalogItems = CatalogItems.Append(model);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostChangeWeights(int weight)
        {
            CatalogItems = new List<CatalogItemViewModel>();
            IEnumerable<CatalogItem> catalogItems = _catalogItemRepository.GetAll()
                .Where(ci => ci.Weight == weight)
                .Take(4);

            foreach (var item in catalogItems)
            {
                var viewModel = CatalogItemConverter.ConvertSingleCatalogItem(item);

                CatalogItems = CatalogItems.Append(viewModel);
            }

            IEnumerable<string> itemsToLoad = new List<string>();

            foreach (var item in CatalogItems)
            {
                string partialView = await _partialToStringRenderer
                    .RenderPartialToStringAsync("_DisplayCatalogItemPartial", item);

                itemsToLoad = itemsToLoad.Append(partialView);
            }

            JsonResult json = new JsonResult(new { ItemsToLoad = itemsToLoad });

            return json;
        }
    }
}
