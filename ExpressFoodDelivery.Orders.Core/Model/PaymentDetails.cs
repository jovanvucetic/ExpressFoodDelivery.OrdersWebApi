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

        public CreditCardPaymentDetails ToCreditCardPayment(decimal amount) =>
            new CreditCardPaymentDetails(amount, CardNumber);
    }
}
