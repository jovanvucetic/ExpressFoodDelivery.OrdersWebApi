using ExpressFoodDelivery.Orders.Core.Model;
using System.Threading.Tasks;

namespace ExpressFoodDelivery.Orders.Core.Interfaces.Repositories
{
    public interface IRestaurantRepository
    {
        Task<AcceptedKitchenResponse> CreateOrderAsync(Order order);
    }
}
