using System;

namespace ExpressFoodDelivery.Orders.Contracts
{
    /// <summary>
    /// Order item contract
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Id of an item from menu
        /// </summary>
        public Guid MenuItemId { get; set; }

        /// <summary>
        /// Item count
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// Price of a single menu item
        /// </summary>
        public decimal ItemPrice { get; set; }
    }
}
