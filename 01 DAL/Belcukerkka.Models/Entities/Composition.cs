using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Belcukerkka.Models.Entities
{
    public class Composition : Entity
    {
        [Range(1, int.MaxValue, ErrorMessage = "Вес должен быть больше 0")]
        [Display(Name = "Вес")]
        public int Weight { get; set; }

        public int WeightTypeId { get; set; }
        [Display(Name = "Тип состава")] public WeightType WeightType { get; set; }

        public List<Box> Boxes { get; set; } = new List<Box>();

        public List<Candy> Candies { get; set; } = new List<Candy>();
        public List<CandyInComposition> CandiesInComposition { get; set; } = new List<CandyInComposition>();
        
    }
}
