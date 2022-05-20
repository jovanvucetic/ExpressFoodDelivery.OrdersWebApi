using ExpressFoodDelivery.Orders.Core.Interfaces.Repositories;
using ExpressFoodDelivery.Orders.Core.Model;
using System.Threading.Tasks;

namespace ExpressFoodDelivery.Orders.Data.DeliveryService
{
    public class DeliveryRepository : IDeliveryRepository
    {
        public Task<AcceptedDeliveryResponse> CreateDeliveryAsync(DeliveryDetails deliveryDetails)
        {
            var response = new AcceptedDeliveryResponse { EstimatedDeliveryTimeInMinutes = 200 };

            return Task.FromResult(response);
        }
    }
}
