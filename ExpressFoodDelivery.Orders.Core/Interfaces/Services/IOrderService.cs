using ExpressFoodDelivery.Orders.Core.Model;
using System.Threading.Tasks;

namespace ExpressFoodDelivery.Orders.Core.Interfaces.Services
{
    public interface IOrderService
    {
        public Task<AcceptedOrderDetails> OrderAsync(Order orderDetails);
    }
}
