namespace ExpressFoodDelivery.Orders.Core.Model
{
    public class PaymentDetails
    {
        public PaymentMethod PaymentMethod { get; }

        public string CardNumber { get; }

        public PaymentDetails(PaymentMethod paymentMethod, string cardNumber)
        {
            PaymentMethod = paymentMethod;
            CardNumber = cardNumber;
        }

        public CreditCardPayment ToCreditCardPayment(decimal amount) =>
            new CreditCardPayment(amount, CardNumber);
    }
}
