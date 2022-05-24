using ExpressFoodDelivery.Orders.Core.Interfaces.Repositories;
using ExpressFoodDelivery.Orders.Core.Model;
using System.Threading.Tasks;

namespace ExpressFoodDelivery.Orders.Data.PaymentService
{
    public class PaymentRepository : IPaymentRepository
    {
        public Task<bool> AuthoriseCreditCardAsync(CreditCardPaymentDetails payment)
        {
            return Task.FromResult(true);
        }

        public Task ExecutePaymentAsync(CreditCardPaymentDetails payment)
        {
            return Task.CompletedTask;
        }
    }
}
