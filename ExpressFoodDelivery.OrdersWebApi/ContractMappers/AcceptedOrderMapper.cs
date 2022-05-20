using ExpressFoodDelivery.Orders.Contracts;
using ExpressFoodDelivery.Orders.Core.Model;

namespace ExpressFoodDelivery.Orders.WebApi.ContractMappers
{
    public static class AcceptedOrderMapper
    {
        public static AcceptedOrderResponse Map(AcceptedOrderDetails response)
            => new AcceptedOrderResponse
            {
                OrderId = response.OrderId,
                OrderAcceptedOn = response.OrderAcceptedOn,
                DeliveryExpectedOn = response.DeliveryExpectedOn
            };
    }
}
