using System;
using System.Collections.Generic;

namespace ExpressFoodDelivery.Orders.Core.Model
{
    public class Order
    {
        public IEnumerable<OrderItem> OrderItems { get; }

        public DeliveryDetails DeliveryDetails { get; }

        public PaymentDetails PaymentDetails { get; }

        public DateTime OrderTime { get; }

        public Order(IEnumerable<OrderItem> orderItems, DeliveryDetails deliveryDetails, PaymentDetails paymentDetails)
        {
            OrderItems = orderItems;
            DeliveryDetails = deliveryDetails;
            PaymentDetails = paymentDetails;
            OrderTime = DateTime.Now;
        }
    }
}
