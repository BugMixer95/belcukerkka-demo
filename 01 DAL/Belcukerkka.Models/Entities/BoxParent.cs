using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Belcukerkka.Models.Entities
{
    public class BoxParent : Entity
    {
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Изображение")]
        public string ImagePath { get; set; }

        public int BoxPackageId { get; set; }
        [Display(Name = "Тип упаковки")] public BoxPackage BoxPackage { get; set; }

        public List<Box> Boxes { get; set; }
    }
}
