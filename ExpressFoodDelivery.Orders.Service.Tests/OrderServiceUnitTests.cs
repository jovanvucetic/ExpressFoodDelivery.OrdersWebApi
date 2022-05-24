using ExpressFoodDelivery.Orders.Core.Exceptions;
using ExpressFoodDelivery.Orders.Core.Interfaces.Repositories;
using ExpressFoodDelivery.Orders.Core.Model;
using ExpressFoodDelivery.Orders.Service.Implementation;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ExpressFoodDelivery.Orders.Service.Tests
{
    public class OrderServiceUnitTests
    {
        [Test]
        public void OrderAsync_OrderDetailsObjectIsNull_InvalidOrderDetailsExceptionThrown()
        {
            var deliveryRepository = Substitute.For<IDeliveryRepository>();
            var paymentRepository = Substitute.For<IPaymentRepository>();
            var restaurantRepository = Substitute.For<IRestaurantRepository>();
            var orderRepository = Substitute.For<IOrderRepository>();

            var targetService = new OrderService(deliveryRepository, paymentRepository, restaurantRepository, orderRepository);

            Assert.ThrowsAsync<InvalidOrderDetailsException>(() => targetService.OrderAsync(null));
        }

        [Test]
        public void OrderAsync_CreditCardAuthorisationFails_CardAuthorizationExceptionThrown()
        {
            var orderDetails = GetOrderDetails();

            var deliveryRepository = Substitute.For<IDeliveryRepository>();
            var paymentRepository = Substitute.For<IPaymentRepository>();
            paymentRepository.AuthoriseCreditCardAsync(Arg.Any<CreditCardPaymentDetails>()).Returns(false);
            var restaurantRepository = Substitute.For<IRestaurantRepository>();
            var orderRepository = Substitute.For<IOrderRepository>();

            var targetService = new OrderService(deliveryRepository, paymentRepository, restaurantRepository, orderRepository);

            Assert.ThrowsAsync<CardAuthorizationException>(() => targetService.OrderAsync(orderDetails));
        }

        [Test]
        public async Task OrderAsync_OrderPlacedCorrectly_AcceptedOrderDetailsReturned()
        {
            var orderDetails = GetOrderDetails();
            var acceptedKitchenResponse = new AcceptedKitchenResponse { EstimatedPreparationTimeInMinutes = 10 };
            var acceptedDeliveryResponse = new AcceptedDeliveryResponse { EstimatedDeliveryTimeInMinutes = 15 };
            var expectedOrderId = Guid.NewGuid();

            var deliveryRepository = Substitute.For<IDeliveryRepository>();
            deliveryRepository.CreateDeliveryAsync(orderDetails.DeliveryDetails).Returns(Task.FromResult(acceptedDeliveryResponse));
            var paymentRepository = Substitute.For<IPaymentRepository>();
            paymentRepository.AuthoriseCreditCardAsync(Arg.Any<CreditCardPaymentDetails>()).Returns(true);
            var restaurantRepository = Substitute.For<IRestaurantRepository>();
            restaurantRepository.CreateOrderAsync(orderDetails).Returns(Task.FromResult(acceptedKitchenResponse));
            var orderRepository = Substitute.For<IOrderRepository>();
            orderRepository.CreateOrderAsync(orderDetails).Returns(expectedOrderId);

            var targetService = new OrderService(deliveryRepository, paymentRepository, restaurantRepository, orderRepository);

            var result = await targetService.OrderAsync(orderDetails);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(AcceptedOrderDetails), result.GetType());
            Assert.AreEqual(expectedOrderId, result.OrderId);
        }

        private Order GetOrderDetails()
        {
            var orderItems = new[] { new OrderItem(Guid.NewGuid(), 1, 349.99M) };
            var pickupAddress = new Address("Diagon Alley", string.Empty, "London", string.Empty, "UK");
            var deliveryAddress = new Address("221b Baker Street", string.Empty, "London", string.Empty, "UK");
            var deliveryDetails = new DeliveryDetails(pickupAddress, deliveryAddress);
            var paymentDetails = new PaymentDetails(PaymentMethod.CreditCard, "4242-4242-4242-4242");
            return new Order(orderItems, deliveryDetails, paymentDetails);
        }

        private class OrderServiceTestContext
        {
            internal IDeliveryRepository DeliveryRepository { get; }
            internal IPaymentRepository PaymentRepository { get; }
            internal IRestaurantRepository RestaurantRepository { get; }
            internal IOrderRepository OrderRepository { get; }

            internal OrderServiceTestContext()
            {
                DeliveryRepository = Substitute.For<IDeliveryRepository>();
                PaymentRepository = Substitute.For<IPaymentRepository>();
                RestaurantRepository = Substitute.For<IRestaurantRepository>();
                OrderRepository = Substitute.For<IOrderRepository>();
            }

            internal OrderService CreateTarget()
                => new OrderService(DeliveryRepository, PaymentRepository, RestaurantRepository, OrderRepository);
        }
    }
}
