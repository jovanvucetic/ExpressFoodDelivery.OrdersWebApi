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
                throw new InvalidOrderDetailsException("Order details object cannot be null");

            if (orderDetails.DeliveryDetails is null)
                throw new InvalidOrderDetailsException("Delivery details object cannot be null");

            if (orderDetails.PaymentDetails is null || orderDetails.PaymentDetails.PaymentMethod == PaymentMethod.CreditCard && string.IsNullOrEmpty(orderDetails.PaymentDetails.CardNumber))
                throw new InvalidOrderDetailsException("Payment details are not valid");

            if(orderDetails.OrderItems is null || orderDetails.OrderItems.Count() == 0)
            {
                throw new InvalidOrderDetailsException("Order items must be defined");
            }

            foreach(var item in orderDetails.OrderItems)
            {
                if (item is null && item.ItemCount <= 0)
                    throw new InvalidOrderDetailsException("Order item count must be higher than 0");
            }

            var price = 0M;
            foreach (var item in orderDetails.OrderItems)
            {
                price += item.ItemPrice * item.ItemCount;
            }

            //Adding delivery fee
            price += 199.99M;

            if (orderDetails.PaymentDetails.PaymentMethod == PaymentMethod.CreditCard)
            {
                if (!await _paymentRepository.AuthoriseCreditCardAsync(orderDetails.PaymentDetails.ToCreditCardPayment(price)))
                {
                    throw new CardAuthorizationException();
                }
            }

            var restaurantResponseTask = _restaurantRepository.CreateOrderAsync(orderDetails);
            var deliveryResponseTask = _deliveryRepository.CreateDeliveryAsync(orderDetails.DeliveryDetails);
            var paymentTask = _paymentRepository.ExecutePaymentAsync(orderDetails.PaymentDetails.ToCreditCardPayment(price));
            var orderTask = _orderRepository.CreateOrderAsync(orderDetails);
            
            await Task.WhenAll(restaurantResponseTask, deliveryResponseTask, paymentTask, orderTask);

            var now = DateTime.Now;
            var estimatedDeliveryTime = now.AddMinutes(restaurantResponseTask.Result.EstimatedPreparationTimeInMinutes).AddMinutes(deliveryResponseTask.Result.EstimatedDeliveryTimeInMinutes);

            return new AcceptedOrderDetails(orderTask.Result, now, estimatedDeliveryTime);
        }
    }
}
