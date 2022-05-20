using System;

namespace ExpressFoodDelivery.Orders.Contracts
{
    /// <summary>
    /// Order response
    /// </summary>
    public class AcceptedOrderResponse
    {
        /// <summary>
        /// Id of placed order
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Date and time of order acceptance
        /// </summary>
        public DateTime OrderAcceptedOn { get; set; }

        /// <summary>
        /// Delivery expected on 
        /// </summary>
        public DateTime DeliveryExpectedOn { get; set; }
    }
}
