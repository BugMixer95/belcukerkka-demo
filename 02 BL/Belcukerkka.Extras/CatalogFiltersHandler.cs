using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Belcukerkka.Services
{
    /// <summary>
    /// Specifies the actions used for filter initialization on Catalog page.
    /// </summary>
    public class CatalogFiltersHandler : IDisposable
    {
        public CatalogFiltersHandler(IEntityRepository<Composition> compositionRepository,
            IEntityRepository<Box> boxRepository)
        {
            _compositionRepository = compositionRepository;
            _boxRepository = boxRepository;
        }

        private readonly IEntityRepository<Composition> _compositionRepository;
        private readonly IEntityRepository<Box> _boxRepository;

        /// <summary>
        /// Gets a distincted list of all weights presented in the database.
        /// </summary>
        /// <returns>List of available weights.</returns>
        public IEnumerable<int> GetAvailableWeights()
        {
            var list = _compositionRepository.GetAllWithDependencies()
                .Where(c => c.Boxes.Count() > 0)
                .Select(c => c.Weight)
                .Distinct();

            return list;
        }

        /// <summary>
        /// Gets min and max box prices from the database to use in Catalog page filter inputs as default ones.
        /// </summary>
        /// <param name="minPrice">The min price in the database.</param>
        /// <param name="maxPrice">The max price in the database.</param>
        public void GetMinAndMaxPrices(out double minPrice, out double maxPrice)
        {
            var pricesList = _boxRepository.GetAll().Select(b => b.Price).ToList();

            pricesList.Sort();

            minPrice = 0;
            maxPrice = 0;

            if (pricesList.Count > 0)
            {
                minPrice = pricesList[0];
                maxPrice = pricesList[pricesList.Count - 1];
            }
        }

        public void Dispose()
        {
        }
    }
}
