using ExpressFoodDelivery.Orders.Core.Interfaces.Repositories;
using ExpressFoodDelivery.Orders.Core.Model;
using System.Threading.Tasks;

namespace ExpressFoodDelivery.Orders.Data.RestaurantService
{
    public class RestaurantRepository : IRestaurantRepository
    {
        public Task<AcceptedKitchenResponse> CreateOrderAsync(Order order)
        {
            return Task.FromResult(new AcceptedKitchenResponse { EstimatedPreparationTimeInMinutes = 200 });
        }
    }
}
