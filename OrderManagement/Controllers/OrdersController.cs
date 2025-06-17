using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.DTOs;
using OrderManagement.Models;
using OrderManagement.Services;

namespace OrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Creates an order
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = new Order
            {
                CustomerId = dto.CustomerId,
                Items = dto.Items.Select(x => new OrderItem
                {
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice
                }).ToList()
            };

            var created = await _orderService.CreateOrderAsync(order);
            return Ok(created);
        }


        /// <summary>
        /// Updates order status.
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] OrderStatus status)
        {
            var updated = await _orderService.UpdateStatusAsync(id, status);
            return updated ? Ok() : BadRequest("Invalid transition or order not found.");
        }

        /// <summary>
        /// Fulfills an order if it is pending.
        /// </summary>
        /// <param name="id">Order ID</param>
        [HttpPost("{id}/fulfill")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FulfillOrder(int id)
        {
            var order = await _orderService.FulfillOrderAsync(id);
            return Ok(order);
        }


        /// <summary>
        /// Returns order analytics.
        /// </summary>
        /// <returns>Average order value and fulfillment time.</returns>
        /// <response code="200">Success</response>
        [ProducesResponseType(typeof(OrderAnalyticsDto), StatusCodes.Status200OK)]
        [HttpGet("analytics")]
        public async Task<ActionResult<OrderAnalyticsDto>> GetAnalytics()
        {
            return Ok(await _orderService.GetAnalyticsAsync());
        }

    }
}
