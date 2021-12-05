using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Belcukerkka.Models.Entities
{
    public class WeightType : Entity
    {
        [Required]
        public string Name { get; set; }

        public List<Composition> Compositions { get; set; } = new List<Composition>();
    }
}
