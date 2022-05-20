using ExpressFoodDelivery.Orders.Core.Model;
using System;
using System.Threading.Tasks;

namespace ExpressFoodDelivery.Orders.Core.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Guid> CreateOrderAsync(Order order);
    }
}
