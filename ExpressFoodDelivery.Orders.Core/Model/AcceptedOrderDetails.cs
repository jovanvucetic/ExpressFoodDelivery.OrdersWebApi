using System;

namespace ExpressFoodDelivery.Orders.Core.Model
{
    public class AcceptedOrderDetails
    {
        public Guid OrderId { get; }

        public DateTime OrderAcceptedOn { get; }

        public DateTime DeliveryExpectedOn { get; }

        public AcceptedOrderDetails(Guid orderId, DateTime orderAcceptedOn, DateTime deliveryExpectedOn)
        {
            OrderId = orderId;
            OrderAcceptedOn = orderAcceptedOn;
            DeliveryExpectedOn = deliveryExpectedOn;
        }
    }
}
