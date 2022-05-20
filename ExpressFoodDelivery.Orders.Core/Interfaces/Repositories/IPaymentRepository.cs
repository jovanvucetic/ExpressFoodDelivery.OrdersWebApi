using ExpressFoodDelivery.Orders.Core.Model;
using System.Threading.Tasks;

namespace ExpressFoodDelivery.Orders.Core.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task ExecutePaymentAsync(CreditCardPayment payment);

        Task<bool> AuthoriseCreditCardAsync(CreditCardPayment payment);
    }
}
