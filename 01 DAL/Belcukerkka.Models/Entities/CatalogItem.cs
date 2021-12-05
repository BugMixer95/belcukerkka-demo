using System.Collections.Generic;

namespace Belcukerkka.Models.Entities
{
    public class CatalogItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int Weight { get; set; }
        public string ChildBoxes { get; set; }
    }
}
