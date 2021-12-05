using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Belcukerkka.Models.Entities
{
    public class CandyInComposition
    {
        public int CandyId { get; set; }
        public Candy Candy { get; set; }

        public int CompositionId { get; set; }
        public Composition Composition { get; set; }

        [Range(1, int.MaxValue)]
        public int Amount { get; set; }
    }
}
