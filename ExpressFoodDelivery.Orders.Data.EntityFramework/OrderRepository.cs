using ExpressFoodDelivery.Orders.Core.Interfaces.Repositories;
using ExpressFoodDelivery.Orders.Core.Model;
using System;
using System.Threading.Tasks;

namespace ExpressFoodDelivery.Orders.Data.EntityFramework
{
    public class OrderRepository : IOrderRepository
    {
        public Task<Guid> CreateOrderAsync(Order order)
        {
            return Task.FromResult(Guid.NewGuid());
        }
    }
}
