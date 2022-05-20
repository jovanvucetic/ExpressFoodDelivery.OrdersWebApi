namespace ExpressFoodDelivery.Orders.Contracts
{
    /// <summary>
    /// Details of delivery
    /// </summary>
    public class DeliveryDetails
    {
        /// <summary>
        /// Address of a restaurant
        /// </summary>
        public Address PickUpAddress { get; set; }

        /// <summary>
        /// Address for delivery
        /// </summary>
        public Address DeliveryAddress { get; set; }
    }
}
