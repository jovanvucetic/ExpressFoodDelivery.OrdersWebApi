using ExpressFoodDelivery.Orders.Core.Model;
using System.Threading.Tasks;

namespace ExpressFoodDelivery.Orders.Core.Interfaces.Repositories
{
    public interface IDeliveryRepository
    {
        Task<AcceptedDeliveryResponse> CreateDeliveryAsync(DeliveryDetails deliveryDetails);
    }
}
