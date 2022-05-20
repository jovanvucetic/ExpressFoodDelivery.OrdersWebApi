namespace ExpressFoodDelivery.Orders.Core.Model
{
    public class CreditCardPayment
    {
        public decimal Amount { get; }

        public string CardNumber { get; }

        public CreditCardPayment(decimal amount, string cardNumber)
        {
            Amount = amount;
            CardNumber = cardNumber;
        }
    }
}
