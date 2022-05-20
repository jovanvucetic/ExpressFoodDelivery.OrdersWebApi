using System;

namespace ExpressFoodDelivery.Orders.Core.Model
{
    public class OrderItem
    {
        public Guid MenuItemId { get; }

        public int ItemCount { get; }

        public decimal ItemPrice { get; }

        public OrderItem(Guid menuItemId, int itemCount, decimal itemPrice)
        {
            MenuItemId = menuItemId;
            ItemCount = itemCount;
            ItemPrice = itemPrice;
        }
    }
}
