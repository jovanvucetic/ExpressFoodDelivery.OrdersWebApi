using System;

namespace ExpressFoodDelivery.Orders.Core.Model
{
    public class AcceptedOrderDetails
    {
        public Guid OrderId { get; }

        public DateTime OrderAcceptedOn { get; }

        public DateTime DeliveryExpectedOn { get; }

        public string OrderedItemsSummary { get; }

        public AcceptedOrderDetails(Guid orderId, DateTime orderAcceptedOn, DateTime deliveryExpectedOn, string orderedItemsSummary)
        {
            OrderId = orderId;
            OrderAcceptedOn = orderAcceptedOn;
            DeliveryExpectedOn = deliveryExpectedOn;
            OrderedItemsSummary = orderedItemsSummary;
        }
    }
}
