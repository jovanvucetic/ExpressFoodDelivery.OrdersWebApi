namespace ExpressFoodDelivery.Orders.Core.Model
{
    public class CreditCardPaymentDetails
    {
        public decimal Amount { get; }

        public string CardNumber { get; }

        public CreditCardPaymentDetails(decimal amount, string cardNumber)
        {
            Amount = amount;
            CardNumber = cardNumber;
        }
    }
}
