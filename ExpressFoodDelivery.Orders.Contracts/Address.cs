namespace ExpressFoodDelivery.Orders.Contracts
{
    /// <summary>
    /// Contract for Address
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Address line 1
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Address line 2
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Postcode
        /// </summary>
        public string Postcode { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }
    }
}
