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

        ///// <summary>
        ///// Creates a new order and applies discounts.
        ///// </summary>
        //[HttpPost]
        //[ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
        //public async Task<IActionResult> CreateOrder([FromBody] Order order)
        //{
        //    var created = await _orderService.CreateOrderAsync(order);
        //    return CreatedAtAction(nameof(GetOrder), new { id = created.Id }, created);
        //}

        ///// <summary>
        ///// Gets an order by ID.
        ///// </summary>
        //[HttpGet("{id}")]
        //[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetOrder(int id)
        //{
        //    // You can expand this or use EF Include if needed
        //    return Ok(await _orderService.GetOrderByIdAsync(id));
        //}

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
        /// Returns order analytics.
        /// </summary>
        [HttpGet("analytics")]
        public async Task<ActionResult<OrderAnalyticsDto>> GetAnalytics()
        {
            return Ok(await _orderService.GetAnalyticsAsync());
        }
    }
}
