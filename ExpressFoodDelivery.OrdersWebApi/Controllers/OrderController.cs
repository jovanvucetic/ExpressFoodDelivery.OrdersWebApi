using ExpressFoodDelivery.Orders.Contracts;
using ExpressFoodDelivery.Orders.Core.Interfaces.Services;
using ExpressFoodDelivery.Orders.WebApi.ContractMappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExpressFoodDelivery.Orders.WebApi.Controllers
{
    /// <summary>
    /// Controller for orders
    /// </summary>
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint for placing an order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<ActionResult> Post([FromBody]Order order)
        {
            var response = await _orderService.OrderAsync(OrderMapper.Map(order));

            return Ok(AcceptedOrderMapper.Map(response));
        }
    }
}
