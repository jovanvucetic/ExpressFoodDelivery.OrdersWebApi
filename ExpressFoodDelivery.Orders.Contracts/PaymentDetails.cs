namespace ExpressFoodDelivery.Orders.Contracts
{
    /// <summary>
    /// Payment details contract
    /// </summary>
    public class PaymentDetails
    {
        /// <summary>
        /// Method of payment
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Credit card number. Can be null if method of payment is cashe
        /// </summary>
        public string CardNumber { get; set; }
    }
}
