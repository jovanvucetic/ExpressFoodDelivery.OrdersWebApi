using CoreModels = ExpressFoodDelivery.Orders.Core.Model;
using ExpressFoodDelivery.Orders.Contracts;

namespace ExpressFoodDelivery.Orders.WebApi.ContractMappers
{
    public static class OrderItemMapper
    {
        public static CoreModels.OrderItem Map(OrderItem orderItem)
            => new CoreModels.OrderItem(orderItem.MenuItemId, orderItem.ItemCount, orderItem.ItemPrice);
    }
}
