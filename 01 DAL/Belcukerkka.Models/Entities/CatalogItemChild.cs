namespace Belcukerkka.Models.Entities
{
    public class CatalogItemChild
    {
        public int ChildId { get; set; }
        public string WeightTypeName { get; set; }
        public double Price { get; set; }
        public double FullPrice { get; set; }
        public string NameTransliterated { get; set; }
        public string WeightTypeNameTransliterated { get; set; }
    }
}
