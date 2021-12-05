using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Belcukerkka.Models.Entities
{
    public class Box : Entity
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        [Display(Name = "Цена")]
        public double Price { get; set; }

        [Range(0.00, double.MaxValue, ErrorMessage = "Цена не может быть отрицательной")]
        [Display(Name = "Цена без скидки")]
        public double? FullPrice { get; set; }
        
        public int BoxParentId { get; set; }
        public BoxParent BoxParent { get; set; }

        public int? CompositionId { get; set; }
        public Composition Composition { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
