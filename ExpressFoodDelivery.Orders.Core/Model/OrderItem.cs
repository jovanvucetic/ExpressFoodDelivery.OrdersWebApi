using System;

namespace ExpressFoodDelivery.Orders.Core.Model
{
    public class OrderItem
    {
        public Guid MenuItemId { get; }

        public string Name { get; set; }

        public int Count { get; }

        public decimal Price { get; }

        public OrderItem(Guid menuItemId, string name, int count, decimal price)
        {
            MenuItemId = menuItemId;
            Name = name;
            Count = count;
            Price = price;
        }
    }
}
