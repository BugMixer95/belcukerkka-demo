using Belcukerkka.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Belcukerkka.Models.Entities
{
    public class Customer : Entity
    {
        public CustomerType Type { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Details { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Некорректный формат электронной почты")]
        public string Email { get; set; }

        public string ContactPerson { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
