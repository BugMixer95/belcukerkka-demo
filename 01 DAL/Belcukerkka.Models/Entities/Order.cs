using Belcukerkka.Models.Enums;
using System;
using System.Collections.Generic;

namespace Belcukerkka.Models.Entities
{
    public class Order : Entity
    {
        public DateTime Date { get; set; }
        public OrderCreatedBy CreatedBy { get; set; }
        public int? InvoiceNumber { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
