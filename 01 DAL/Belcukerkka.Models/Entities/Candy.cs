using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Belcukerkka.Models.Entities
{
    public class Candy : Entity
    {
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        public string ImagePath { get; set; }

        public List<Composition> Compositions { get; set; } = new List<Composition>();
        public List<CandyInComposition> CandyInCompositions { get; set; } = new List<CandyInComposition>();
    }
}
