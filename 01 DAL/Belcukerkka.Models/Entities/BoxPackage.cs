using System.Collections.Generic;

namespace Belcukerkka.Models.Entities
{
    public class BoxPackage : Entity
    {
        public string Name { get; set; }

        public List<BoxParent> BoxParents { get; set; } = new List<BoxParent>();
    }
}
