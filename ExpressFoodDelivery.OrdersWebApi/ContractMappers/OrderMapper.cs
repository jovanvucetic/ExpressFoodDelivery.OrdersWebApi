using CoreModels = ExpressFoodDelivery.Orders.Core.Model;
using ExpressFoodDelivery.Orders.Contracts;
using System.Linq;

namespace ExpressFoodDelivery.Orders.WebApi.ContractMappers
{
    public static class OrderMapper
    {
        public static CoreModels.Order Map(Order order)
        {
            var orderItems = order.OrderItems.Select(OrderItemMapper.Map).ToArray();
            var deliveryDetails = new CoreModels.DeliveryDetails(AddressMapper.Map(order.DeliveryDetails.PickUpAddress), AddressMapper.Map(order.DeliveryDetails.DeliveryAddress));

            return new CoreModels.Order(orderItems, deliveryDetails, PaymentDetailsMapper.Map(order.PaymentDetails));
        }
    }
}
