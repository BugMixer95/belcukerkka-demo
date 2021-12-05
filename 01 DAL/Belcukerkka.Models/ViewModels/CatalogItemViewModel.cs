using Belcukerkka.Models.Entities;
using System.Collections.Generic;

namespace Belcukerkka.Models.ViewModels
{
    public class CatalogItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int Weight { get; set; }
        public List<CatalogItemChild> Children { get; set; }
    }
}
