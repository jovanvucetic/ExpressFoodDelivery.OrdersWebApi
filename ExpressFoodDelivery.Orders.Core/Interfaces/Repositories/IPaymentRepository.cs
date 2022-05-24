using ExpressFoodDelivery.Orders.Core.Model;
using System.Threading.Tasks;

namespace ExpressFoodDelivery.Orders.Core.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task ExecutePaymentAsync(CreditCardPaymentDetails payment);

        Task<bool> AuthoriseCreditCardAsync(CreditCardPaymentDetails payment);
    }
}
