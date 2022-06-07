using ExpressFoodDelivery.Orders.Core.Exceptions;
using ExpressFoodDelivery.Orders.Core.Interfaces.Repositories;
using ExpressFoodDelivery.Orders.Core.Interfaces.Services;
using ExpressFoodDelivery.Orders.Core.Model;
using System;
using System.Linq;
using System.Text;
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

        public async Task<AcceptedOrderDetails> OrderAsync(Order arg1)
        {
            if (arg1 != null)
            {
                if (arg1.DeliveryDetails != null)
                {
                    if (arg1.PaymentDetails != null && (arg1.PaymentDetails.PaymentMethod != PaymentMethod.CreditCard || !string.IsNullOrEmpty(arg1.PaymentDetails.CardNumber)))
                    {
                        if (arg1.OrderItems != null && arg1.OrderItems.Count() != 0)
                        {
                            foreach (var item in arg1.OrderItems)
                            {
                                if (item is null || item.Count <= 0)
                                {
                                    throw new InvalidOrderDetailsException("Order item count must be higher than 0");
                                }
                            }

                            var temp = 0M;
                            foreach (var item in arg1.OrderItems)
                            {
                                temp += item.Price * item.Count;
                            }

                            //Adding delivery fee
                            temp += 199.99M;

                            if (arg1.PaymentDetails.PaymentMethod == PaymentMethod.CreditCard)
                            {
                                if (!await _paymentRepository.AuthoriseCreditCardAsync(arg1.PaymentDetails.ToCreditCardPayment(temp)))
                                {
                                    throw new CardAuthorizationException();
                                }
                            }

                            var task1 = _restaurantRepository.CreateOrderAsync(arg1);
                            var task2 = _deliveryRepository.CreateDeliveryAsync(arg1.DeliveryDetails);
                            var task3 = _paymentRepository.ExecutePaymentAsync(arg1.PaymentDetails.ToCreditCardPayment(temp));
                            var task4 = _orderRepository.CreateOrderAsync(arg1);

                            await Task.WhenAll(task1, task2, task3, task4);

                            var now = DateTime.Now;
                            var deliveryTime = now.AddMinutes(task1.Result.EstimatedPreparationTimeInMinutes).AddMinutes(task2.Result.EstimatedDeliveryTimeInMinutes);

                            var sb = new StringBuilder("Order summary: \n");
                            foreach (var item in arg1.OrderItems)
                            {
                                var str = string.Format("{0}x, {1} - {2} din\n", item.Count, item.Name, item.Count * item.Price);
                                sb.Append(str);
                            }
                            sb.Append($"Delivery price: {199.99M}");

                            return new AcceptedOrderDetails(task4.Result, now, deliveryTime, sb.ToString());
                        }
                        else
                        {
                            throw new InvalidOrderDetailsException("Order items must be defined");
                        }
                    }
                    else
                    {
                        throw new InvalidOrderDetailsException("Payment details are not valid");
                    }
                }
                else
                {
                    throw new InvalidOrderDetailsException("Delivery details object cannot be null");
                }
            }
            else
            {
                throw new InvalidOrderDetailsException("Order details object cannot be null");
            }
        }
    }
}
