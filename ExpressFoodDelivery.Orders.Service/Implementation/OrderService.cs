using ExpressFoodDelivery.Orders.Core.Exceptions;
using ExpressFoodDelivery.Orders.Core.Interfaces.Repositories;
using ExpressFoodDelivery.Orders.Core.Interfaces.Services;
using ExpressFoodDelivery.Orders.Core.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressFoodDelivery.Orders.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private const decimal DefaultDeliveryPrice = 199.99M;

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
            {
                throw new InvalidOrderDetailsException("Order details object cannot be null");
            }

            var fullDeliveryPrice = orderDetails.OrderItems.Sum(item => item.ItemPrice * item.ItemCount) + DefaultDeliveryPrice;

            if (orderDetails.PaymentDetails.PaymentMethod == PaymentMethod.CreditCard)
            {
                var isCreditCardAuthorized = await _paymentRepository.AuthoriseCreditCardAsync(
                    orderDetails.PaymentDetails.ToCreditCardPayment(fullDeliveryPrice));

                if (!isCreditCardAuthorized)
                {
                    throw new CardAuthorizationException();
                }
            }

            var restaurantResponseTask = _restaurantRepository.CreateOrderAsync(orderDetails);
            var deliveryResponseTask = _deliveryRepository.CreateDeliveryAsync(orderDetails.DeliveryDetails);
            var paymentTask = _paymentRepository.ExecutePaymentAsync(orderDetails.PaymentDetails.ToCreditCardPayment(fullDeliveryPrice));
            var orderTask = _orderRepository.CreateOrderAsync(orderDetails);

            await Task.WhenAll(restaurantResponseTask, deliveryResponseTask, paymentTask, orderTask);

            var orderConfirmedAt = DateTime.Now;
            var estimatedDeliveryTime = orderConfirmedAt.
                AddMinutes(restaurantResponseTask.Result.EstimatedPreparationTimeInMinutes).
                AddMinutes(deliveryResponseTask.Result.EstimatedDeliveryTimeInMinutes);

            return new AcceptedOrderDetails(orderTask.Result, orderConfirmedAt, estimatedDeliveryTime);
        }
    }
}
