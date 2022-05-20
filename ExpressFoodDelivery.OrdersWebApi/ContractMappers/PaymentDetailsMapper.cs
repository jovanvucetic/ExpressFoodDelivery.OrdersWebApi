using CoreModels = ExpressFoodDelivery.Orders.Core.Model;
using ExpressFoodDelivery.Orders.Contracts;
using System;

namespace ExpressFoodDelivery.Orders.WebApi.ContractMappers
{
    public static class PaymentDetailsMapper
    {
        public static CoreModels.PaymentDetails Map(PaymentDetails paymentDetails)
            => new CoreModels.PaymentDetails(Map(paymentDetails.PaymentMethod), paymentDetails.CardNumber);

        private static CoreModels.PaymentMethod Map(PaymentMethod paymentMethod)
            => paymentMethod switch
            {
                PaymentMethod.CreditCard => CoreModels.PaymentMethod.CreditCard,
                PaymentMethod.Cash => CoreModels.PaymentMethod.Cash,
                _ => throw new ArgumentOutOfRangeException($"{paymentMethod} cannot be mapped to payment method")
            };
    }
}
