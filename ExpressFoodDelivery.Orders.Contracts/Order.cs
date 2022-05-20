using System;
using System.Collections.Generic;

namespace ExpressFoodDelivery.Orders.Contracts
{
    /// <summary>
    /// Contract for placing an order
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Collection of ordered items
        /// </summary>
        public IEnumerable<OrderItem> OrderItems { get; set; }

        /// <summary>
        /// Details of delivery
        /// </summary>
        public DeliveryDetails DeliveryDetails { get; set; }

        /// <summary>
        /// Details of payment
        /// </summary>
        public PaymentDetails PaymentDetails { get; set; }

        /// <summary>
        /// Time for which order is placed. If null, order will be marked as "as soon as possible"
        /// </summary>
        public DateTime? OrderedForTime { get; set; }
    }
}
