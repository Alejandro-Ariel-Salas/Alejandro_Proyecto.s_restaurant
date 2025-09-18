using Aplication.Exceptions;
using Aplication.Interfaces;
using Aplication.Models;
using Aplication.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel orderModel)
        {
            try
            {
                var order = await _orderService.CreateOrder(orderModel);
                return CreatedAtAction(nameof(CreateOrder), new { id = order.orderNumber }, order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOrders([FromQuery] string? dateFrom, [FromQuery] string? dateTo, [FromQuery] int? status)
        {
            try
            {
                var orders = await _orderService.GetOrderWithFilter(dateFrom, dateTo, status);
                return Ok(orders);
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }

        [HttpOptions("{id}")]
        [ProducesResponseType(typeof(OrderShowResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(long id)
        {
            try
            {
                var order = await _orderService.GetOrderById(id);
                return Ok(order);
            }
            catch (ExeptionNotFound ex)
            {
                return NotFound(new ApiError { Message = ex.Message });
            }
        }

        [HttpOptions]
        [ProducesResponseType(typeof(OrderUpdateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderItems(long id, [FromBody] OrderModifyModel orderModifyModel)
        {
            try
            {
                var order = await _orderService.UpdateItems(id, orderModifyModel);
                return Ok(order);
            }
            catch (ExeptionNotFound ex)
            {
                return NotFound(new ApiError { Message = ex.Message });
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }

        [HttpOptions("{id}/item/{orderItemId}")]
        [ProducesResponseType(typeof(OrderUpdateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderItemStatus(long id, long orderItemId, [FromBody] StatusModifyModel status)
        {
            try
            {
                var order = await _orderService.UpdateOrderItemStatus(id, orderItemId, status);
                return Ok(order);
            }
            catch (ExeptionNotFound ex)
            {
                return NotFound(new ApiError { Message = ex.Message });
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }
    }
}
