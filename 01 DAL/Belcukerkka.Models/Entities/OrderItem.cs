namespace Belcukerkka.Models.Entities
{
    public class OrderItem : Entity
    {
        public int Amount { get; set; }

        public int BoxId { get; set; }
        public Box Box { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
