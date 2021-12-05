using Belcukerkka.Models.Entities;
using Belcukerkka.Models.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Belcukerkka.Services
{
    public sealed class CatalogItemConverter
    {
        /// <summary>
        /// Converts a catalog item taken from the database to a CatalogItemViewModel object.
        /// </summary>
        /// <param name="item">Catalog item that should be converted.</param>
        /// <returns>CatalogItemViewModel object converted from a specified item.</returns>
        public static CatalogItemViewModel ConvertSingleCatalogItem(CatalogItem item)
        {
            var children = JsonConvert.DeserializeObject<IEnumerable<CatalogItemChild>>(item.ChildBoxes)
                    .OrderBy(cic => cic.WeightTypeName)
                    .ToList();

            foreach (var child in children)
            {
                child.NameTransliterated = StringTransliterator.FromRussianToEnglish(item.Name);
                child.WeightTypeNameTransliterated = StringTransliterator.FromRussianToEnglish(child.WeightTypeName);
            }

            var model = new CatalogItemViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                ImagePath = item.ImagePath,
                Weight = item.Weight,
                Children = children
            };

            return model;
        }

        /// <summary>
        /// Converts a IEnumerable of catalog items taken from the database to a IEnumerable of CatalogItemViewModel objects.
        /// </summary>
        /// <param name="items">List of catalog items to be processed.</param>
        /// <returns>IEnumerable of converted CatalogItemViewModel objects.</returns>
        public static IEnumerable<CatalogItemViewModel> ConvertCatalogItemsList(IEnumerable<CatalogItem> items)
        {
            foreach (var item in items)
                yield return ConvertSingleCatalogItem(item);
        }
    }
}
