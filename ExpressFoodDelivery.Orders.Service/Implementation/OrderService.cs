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
            ValidateOrderDetails(orderDetails);

            var totalPaymentAmount = CalculateTotalPaymentAmount(orderDetails.OrderItems);

            await ValidatePaymentMethodAsync(orderDetails.PaymentDetails, totalPaymentAmount);

            var restaurantResponse = _restaurantRepository.CreateOrderAsync(orderDetails);
            var deliveryResponse = _deliveryRepository.CreateDeliveryAsync(orderDetails.DeliveryDetails);
            var payment = _paymentRepository.ExecutePaymentAsync(orderDetails.PaymentDetails.ToCreditCardPayment(totalPaymentAmount));
            var order = _orderRepository.CreateOrderAsync(orderDetails);

            await Task.WhenAll(restaurantResponse, deliveryResponse, payment, order);

            var orderAcceptedOn = DateTime.Now;
            var estimatedDeliveryTime = CalculateEstimatedDeliveryTime(orderAcceptedOn, restaurantResponse.Result, deliveryResponse.Result);

            return new AcceptedOrderDetails(order.Result, orderAcceptedOn, estimatedDeliveryTime);
        }

        private DateTime CalculateEstimatedDeliveryTime(DateTime orderAcceptedOn, AcceptedKitchenResponse kitchenResponse, AcceptedDeliveryResponse deliveryResponse)
            => orderAcceptedOn.AddMinutes(kitchenResponse.EstimatedPreparationTimeInMinutes).AddMinutes(deliveryResponse.EstimatedDeliveryTimeInMinutes);

        private async Task ValidatePaymentMethodAsync(PaymentDetails paymentDetails, decimal totalPaymentAmount)
        {
            if (paymentDetails.PaymentMethod == PaymentMethod.Cash)
            {
                return;
            }

            var successfullyAuthorized = await _paymentRepository.AuthoriseCreditCardAsync(paymentDetails.ToCreditCardPayment(totalPaymentAmount));

            if (!successfullyAuthorized)
            {
                throw new CardAuthorizationException();
            }
        }

        private decimal CalculateTotalPaymentAmount(IEnumerable<OrderItem> orderItems)
        {
            var totalOrderFee = orderItems.Sum(item => item.ItemPrice * item.ItemCount);

            return totalOrderFee + FixedDeliveryFee;
        }

        private void ValidateOrderDetails(Order orderDetails)
        {
            if (orderDetails is null)
                throw new InvalidOrderDetailsException();
        }
    }
}
