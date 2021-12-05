using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Belcukerkka.Models.Entities;
using Belcukerkka.Models.ViewModels;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Belcukerkka.Services;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication.Pages.Catalog
{
    public class IndexModel : PageModel
    {
        public IndexModel(ICatalogItemRepository catalogItemRepository,
            IPartialToStringRenderer partialToStringRenderer,
            IEntityRepository<Composition> compositionRepository,
            IEntityRepository<Box> boxRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _catalogItemRepository = catalogItemRepository;
            _partialToStringRenderer = partialToStringRenderer;
            _compositionRepository = compositionRepository;
            _boxRepository = boxRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        private readonly ICatalogItemRepository _catalogItemRepository;
        private readonly IPartialToStringRenderer _partialToStringRenderer;
        private readonly IEntityRepository<Composition> _compositionRepository;
        private readonly IEntityRepository<Box> _boxRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IEnumerable<CatalogItemViewModel> CatalogItems { get; set; }

        public IEnumerable<int> AvailableWeights { get; set; }

        private int ItemsToLoadCount { get; } = 12;

        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public bool EveryItemIsLoaded { get; set; }

        public IActionResult OnGet(int[] weights, double minPrice, double maxPrice)
        {
            if (_webHostEnvironment.EnvironmentName == "Staging" && !User.Identity.IsAuthenticated)
                return RedirectToPage("/Maintain");

            CatalogItems = new List<CatalogItemViewModel>();
            var catalogItems = _catalogItemRepository.GetAll();

            ViewData["IsFiltered"] = false;

            using (var handler = new CatalogFiltersHandler(_compositionRepository, _boxRepository))
            {
                AvailableWeights = handler.GetAvailableWeights()
                    .OrderBy(w => w)
                    .ToList();

                handler.GetMinAndMaxPrices(out double minPriceAvailable, out double maxPriceAvailable);
                MinPrice = minPriceAvailable;
                MaxPrice = maxPriceAvailable;
            }

            foreach (var item in catalogItems)
            {
                var model = CatalogItemConverter.ConvertSingleCatalogItem(item);

                CatalogItems = CatalogItems.Append(model);
            }

            FilterResults(weights, minPrice, maxPrice);

            int totalItems = CatalogItems.Count();

            CatalogItems = CatalogItems.OrderBy(ci => ci.Id)
                .ThenBy(ci => ci.Weight)
                .Take(ItemsToLoadCount);

            EveryItemIsLoaded = totalItems <= CatalogItems.Count();

            return Page();
        }

        public async Task<IActionResult> OnPostLoadMore(int size, int[] weights, double minPrice, double maxPrice)
        {
            int itemsCount = _catalogItemRepository.GetAll().Count();

            CatalogItems = new List<CatalogItemViewModel>();
            IEnumerable<CatalogItem> catalogItems = _catalogItemRepository.GetAll();
            
            foreach (var item in catalogItems)
            {
                var viewModel = CatalogItemConverter.ConvertSingleCatalogItem(item);

                CatalogItems = CatalogItems.Append(viewModel);
            }

            FilterResults(weights, minPrice, maxPrice);

            CatalogItems = CatalogItems.OrderBy(ci => ci.Id)
                .ThenBy(ci => ci.Weight)
                .Skip(size)
                .Take(ItemsToLoadCount);

            IEnumerable<string> itemsToLoad = new List<string>();

            foreach (var item in CatalogItems)
            {
                string partialView = await _partialToStringRenderer
                    .RenderPartialToStringAsync("_DisplayCatalogItemPartial", item);

                itemsToLoad = itemsToLoad.Append(partialView);
            }

            JsonResult json = new JsonResult(new { ItemsToLoad = itemsToLoad, ItemsCount = itemsCount });

            return json;
        }

        private void FilterResults(int[] weights, double minPrice, double maxPrice)
        {
            if (weights.Length != 0)
            {
                CatalogItems = CatalogItems.Where(ci => weights.Contains(ci.Weight));
                ViewData["WeightsFilter"] = weights;
                ViewData["IsFiltered"] = true;
            }

            if (minPrice != 0)
            {
                IEnumerable<CatalogItemViewModel> list = new List<CatalogItemViewModel>();

                foreach (var parent in CatalogItems)
                {
                    foreach (var child in parent.Children)
                    {
                        if (child.Price >= minPrice)
                        {
                            list = list.Append(parent);
                        }
                    }
                }

                CatalogItems = list.Distinct();
                ViewData["MinPriceFilter"] = minPrice;
                ViewData["IsFiltered"] = true;
            }

            if (maxPrice != 0)
            {
                IEnumerable<CatalogItemViewModel> list = new List<CatalogItemViewModel>();

                foreach (var parent in CatalogItems)
                {
                    foreach (var child in parent.Children)
                    {
                        if (child.Price <= maxPrice)
                        {
                            list = list.Append(parent);
                        }
                    }
                }

                CatalogItems = list.Distinct();
                ViewData["MaxPriceFilter"] = maxPrice;
                ViewData["IsFiltered"] = true;
            }
        }
    }
}
