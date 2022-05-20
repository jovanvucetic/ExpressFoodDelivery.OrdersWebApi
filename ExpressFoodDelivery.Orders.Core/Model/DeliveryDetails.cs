using System;

namespace ExpressFoodDelivery.Orders.Core.Model
{
    public class DeliveryDetails
    {
        public Address PickUpAddress { get; }

        public Address DeliveryAddress { get; }

        public DeliveryDetails(Address pickUpAddress, Address deliveryAddress)
        {
            PickUpAddress = pickUpAddress;
            DeliveryAddress = deliveryAddress;
        }
    }
}
