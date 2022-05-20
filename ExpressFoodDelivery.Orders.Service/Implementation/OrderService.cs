using ExpressFoodDelivery.Orders.Core.Exceptions;
using ExpressFoodDelivery.Orders.Core.Interfaces.Repositories;
using ExpressFoodDelivery.Orders.Core.Interfaces.Services;
using ExpressFoodDelivery.Orders.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressFoodDelivery.Orders.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private const decimal FixedDeliveryFee = 199.99M;

        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IDeliveryRepository deliveryRepository, IPaymentRepository paymentRepository,
            IRestaurantRepository restaurantRepository, IOrderRepository orderRepository)
        {
            _deliveryRepository = deliveryRepository;
            _paymentRepository = paymentRepository;
            _restaurantRepository = restaurantRepository;
            _orderRepository = orderRepository;
        }

        public async Task<AcceptedOrderDetails> OrderAsync(Order orderDetails)
        {
            if (orderDetails is null)
                throw new InvalidOrderDetailsException();

            var price = Decimal.Zero;
            foreach(var item in orderDetails.OrderItems)
            {
                price += item.ItemPrice * item.ItemCount;
            }

            if (orderDetails.PaymentDetails.PaymentMethod == PaymentMethod.CreditCard)
            {
                var successfullyAuthorized = await _paymentRepository.AuthoriseCreditCardAsync(orderDetails.PaymentDetails.ToCreditCardPayment(price));

                if (!successfullyAuthorized)
                {
                    throw new CardAuthorizationException();
                }
            }

            var restaurantResponse = _restaurantRepository.CreateOrderAsync(orderDetails);
            var deliveryResponse = _deliveryRepository.CreateDeliveryAsync(orderDetails.DeliveryDetails);
            var payment = _paymentRepository.ExecutePaymentAsync(orderDetails.PaymentDetails.ToCreditCardPayment(price));
            var order = _orderRepository.CreateOrderAsync(orderDetails);

            await Task.WhenAll(restaurantResponse, deliveryResponse, payment, order);

            var orderAcceptedOn = DateTime.Now;
            var estimatedDeliveryTime = orderAcceptedOn.AddMinutes(restaurantResponse.Result.EstimatedPreparationTimeInMinutes).AddMinutes(deliveryResponse.Result.EstimatedDeliveryTimeInMinutes);

            return new AcceptedOrderDetails(order.Result, orderAcceptedOn, estimatedDeliveryTime);
        }
    }
}
