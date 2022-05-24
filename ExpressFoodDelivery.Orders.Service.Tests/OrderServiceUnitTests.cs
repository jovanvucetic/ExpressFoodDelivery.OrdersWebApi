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
            var testContext = new OrderServiceTestContext();

            var targetService = testContext.CreateTarget();

            Assert.ThrowsAsync<InvalidOrderDetailsException>(() => targetService.OrderAsync(null));
        }

        [Test]
        public void OrderAsync_CreditCardAuthorisationFails_CardAuthorizationExceptionThrown()
        {
            var orderDetails = GetOrderDetails();

            var testContext = new OrderServiceTestContext();
            testContext.PaymentRepository.AuthoriseCreditCardAsync(Arg.Any<CreditCardPaymentDetails>()).Returns(false);

            var targetService = testContext.CreateTarget();

            Assert.ThrowsAsync<CardAuthorizationException>(() => targetService.OrderAsync(orderDetails));
        }

        [Test]
        public void OrderAsync_DeliveryDetailsNotProvided_InvalidOrderDetailsExceptionThrown()
        {
            var orderItems = new[] { new OrderItem(Guid.NewGuid(), "Pizza", 1, 349.99M) };
            var paymentDetails = new PaymentDetails(PaymentMethod.CreditCard, "4242-4242-4242-4242");
            var orderDetails = new Order(orderItems, null, paymentDetails);

            var testContext = new OrderServiceTestContext();
            testContext.PaymentRepository.AuthoriseCreditCardAsync(Arg.Any<CreditCardPaymentDetails>()).Returns(true);

            var targetService = testContext.CreateTarget();

            Assert.ThrowsAsync<InvalidOrderDetailsException>(() => targetService.OrderAsync(orderDetails));
        }

        [Test]
        public void OrderAsync_PaymentDetailsNotProvided_InvalidOrderDetailsExceptionThrown()
        {
            var orderItems = new[] { new OrderItem(Guid.NewGuid(), "Pizza", 1, 349.99M) };
            var pickupAddress = new Address("Diagon Alley", string.Empty, "London", string.Empty, "UK");
            var deliveryAddress = new Address("221b Baker Street", string.Empty, "London", string.Empty, "UK");
            var deliveryDetails = new DeliveryDetails(pickupAddress, deliveryAddress);
            var orderDetails = new Order(orderItems, deliveryDetails, null);

            var testContext = new OrderServiceTestContext();
            testContext.PaymentRepository.AuthoriseCreditCardAsync(Arg.Any<CreditCardPaymentDetails>()).Returns(true);

            var targetService = testContext.CreateTarget();

            Assert.ThrowsAsync<InvalidOrderDetailsException>(() => targetService.OrderAsync(orderDetails));
        }

        [Test]
        public void OrderAsync_PaymentDetailsInvalid_InvalidOrderDetailsExceptionThrown()
        {
            var orderItems = new[] { new OrderItem(Guid.NewGuid(), "Pizza", 1, 349.99M) };
            var pickupAddress = new Address("Diagon Alley", string.Empty, "London", string.Empty, "UK");
            var deliveryAddress = new Address("221b Baker Street", string.Empty, "London", string.Empty, "UK");
            var deliveryDetails = new DeliveryDetails(pickupAddress, deliveryAddress);
            var paymentDetails = new PaymentDetails(PaymentMethod.CreditCard, string.Empty);
            var orderDetails = new Order(orderItems, deliveryDetails, paymentDetails);

            var testContext = new OrderServiceTestContext();
            testContext.PaymentRepository.AuthoriseCreditCardAsync(Arg.Any<CreditCardPaymentDetails>()).Returns(true);

            var targetService = testContext.CreateTarget();

            Assert.ThrowsAsync<InvalidOrderDetailsException>(() => targetService.OrderAsync(orderDetails));
        }

        [Test]
        public void OrderAsync_OrderItemsNotProvided_InvalidOrderDetailsExceptionThrown()
        {
            var pickupAddress = new Address("Diagon Alley", string.Empty, "London", string.Empty, "UK");
            var deliveryAddress = new Address("221b Baker Street", string.Empty, "London", string.Empty, "UK");
            var deliveryDetails = new DeliveryDetails(pickupAddress, deliveryAddress);
            var paymentDetails = new PaymentDetails(PaymentMethod.CreditCard, "4242-4242-4242-4242");
            var orderDetails = new Order(null, deliveryDetails, paymentDetails);

            var testContext = new OrderServiceTestContext();
            testContext.PaymentRepository.AuthoriseCreditCardAsync(Arg.Any<CreditCardPaymentDetails>()).Returns(true);

            var targetService = testContext.CreateTarget();

            Assert.ThrowsAsync<InvalidOrderDetailsException>(() => targetService.OrderAsync(orderDetails));
        }

        [Test]
        public void OrderAsync_OrderItemsCollectionIsEmpty_InvalidOrderDetailsExceptionThrown()
        {
            var orderItems = Array.Empty<OrderItem>();
            var pickupAddress = new Address("Diagon Alley", string.Empty, "London", string.Empty, "UK");
            var deliveryAddress = new Address("221b Baker Street", string.Empty, "London", string.Empty, "UK");
            var deliveryDetails = new DeliveryDetails(pickupAddress, deliveryAddress);
            var paymentDetails = new PaymentDetails(PaymentMethod.CreditCard, "4242-4242-4242-4242");
            var orderDetails = new Order(orderItems, deliveryDetails, paymentDetails);

            var testContext = new OrderServiceTestContext();
            testContext.PaymentRepository.AuthoriseCreditCardAsync(Arg.Any<CreditCardPaymentDetails>()).Returns(true);

            var targetService = testContext.CreateTarget();

            Assert.ThrowsAsync<InvalidOrderDetailsException>(() => targetService.OrderAsync(orderDetails));
        }

        [Test]
        public void OrderAsync_OrderItemCollectionContainsNull_InvalidOrderDetailsExceptionThrown()
        {
            var orderItems = new[] { new OrderItem(Guid.NewGuid(), "Pizza", 1, 349.99M), null };
            var pickupAddress = new Address("Diagon Alley", string.Empty, "London", string.Empty, "UK");
            var deliveryAddress = new Address("221b Baker Street", string.Empty, "London", string.Empty, "UK");
            var deliveryDetails = new DeliveryDetails(pickupAddress, deliveryAddress);
            var paymentDetails = new PaymentDetails(PaymentMethod.CreditCard, "4242-4242-4242-4242");
            var orderDetails = new Order(orderItems, deliveryDetails, paymentDetails);

            var testContext = new OrderServiceTestContext();
            testContext.PaymentRepository.AuthoriseCreditCardAsync(Arg.Any<CreditCardPaymentDetails>()).Returns(true);

            var targetService = testContext.CreateTarget();

            Assert.ThrowsAsync<InvalidOrderDetailsException>(() => targetService.OrderAsync(orderDetails));
        }

        [Test]
        public void OrderAsync_OrderItemHasZeroCount_InvalidOrderDetailsExceptionThrown()
        {
            var orderItems = new[] { new OrderItem(Guid.NewGuid(), "Pizza", 0, 349.99M) };
            var pickupAddress = new Address("Diagon Alley", string.Empty, "London", string.Empty, "UK");
            var deliveryAddress = new Address("221b Baker Street", string.Empty, "London", string.Empty, "UK");
            var deliveryDetails = new DeliveryDetails(pickupAddress, deliveryAddress);
            var paymentDetails = new PaymentDetails(PaymentMethod.CreditCard, "4242-4242-4242-4242");
            var orderDetails = new Order(orderItems, deliveryDetails, paymentDetails);

            var testContext = new OrderServiceTestContext();
            testContext.PaymentRepository.AuthoriseCreditCardAsync(Arg.Any<CreditCardPaymentDetails>()).Returns(true);

            var targetService = testContext.CreateTarget();

            Assert.ThrowsAsync<InvalidOrderDetailsException>(() => targetService.OrderAsync(orderDetails));
        }

        [Test]
        public async Task OrderAsync_OrderPlacedCorrectly_AcceptedOrderDetailsReturned()
        {
            var orderDetails = GetOrderDetails();
            var acceptedKitchenResponse = new AcceptedKitchenResponse { EstimatedPreparationTimeInMinutes = 10 };
            var acceptedDeliveryResponse = new AcceptedDeliveryResponse { EstimatedDeliveryTimeInMinutes = 15 };
            var expectedOrderId = Guid.NewGuid();

            var testContext = new OrderServiceTestContext();
            testContext.DeliveryRepository.CreateDeliveryAsync(orderDetails.DeliveryDetails).Returns(Task.FromResult(acceptedDeliveryResponse));
            testContext.PaymentRepository.AuthoriseCreditCardAsync(Arg.Any<CreditCardPaymentDetails>()).Returns(true);
            testContext.RestaurantRepository.CreateOrderAsync(orderDetails).Returns(Task.FromResult(acceptedKitchenResponse));
            testContext.OrderRepository.CreateOrderAsync(orderDetails).Returns(expectedOrderId);

            var targetService = testContext.CreateTarget();

            var result = await targetService.OrderAsync(orderDetails);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(AcceptedOrderDetails), result.GetType());
            Assert.AreEqual(expectedOrderId, result.OrderId);
        }

        private Order GetOrderDetails()
        {
            var orderItems = new[] { new OrderItem(Guid.NewGuid(),"Pizza", 1, 349.99M) };
            var pickupAddress = new Address("Diagon Alley", string.Empty, "London", string.Empty, "UK");
            var deliveryAddress = new Address("221b Baker Street", string.Empty, "London", string.Empty, "UK");
            var deliveryDetails = new DeliveryDetails(pickupAddress, deliveryAddress);
            var paymentDetails = new PaymentDetails(PaymentMethod.CreditCard, "4242-4242-4242-4242");
            return new Order(orderItems, deliveryDetails, paymentDetails);
        }

        private class OrderServiceTestContext
        {
            public IDeliveryRepository DeliveryRepository { get; }
            public IPaymentRepository PaymentRepository { get; }
            public IRestaurantRepository RestaurantRepository { get; }
            public IOrderRepository OrderRepository { get; }

            public OrderServiceTestContext()
            {
                DeliveryRepository = Substitute.For<IDeliveryRepository>();
                PaymentRepository = Substitute.For<IPaymentRepository>();
                RestaurantRepository = Substitute.For<IRestaurantRepository>();
                OrderRepository = Substitute.For<IOrderRepository>();
            }

            public OrderService CreateTarget()
                => new OrderService(DeliveryRepository, PaymentRepository, RestaurantRepository, OrderRepository);
        }
    }
}
