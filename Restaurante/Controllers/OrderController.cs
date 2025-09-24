
using Aplication.Exceptions;
using Aplication.Interfaces;
using Aplication.Models;
using Aplication.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("Order")]
        [ProducesResponseType(typeof(OrderCreateReponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderModel)
        {
            try
            {
                var order = await _orderService.CreateOrder(orderModel);
                return CreatedAtAction(nameof(CreateOrder), new { id = order.OrderNumber }, order);
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

        [HttpGet("Order")]
        [ProducesResponseType(typeof(List<OrderCreateReponse>), StatusCodes.Status200OK)]
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

        [HttpGet("Order/{id}")]
        [ProducesResponseType(typeof(OrderDetailResponse), StatusCodes.Status200OK)]
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

        [HttpPut("Order/{id}")]
        [ProducesResponseType(typeof(OrderUpdateReponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderItems(long id, [FromBody] OrderUpdateRequest orderModifyModel)
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

        [HttpPut("Order/{id}/item/{orderItemId}")]
        [ProducesResponseType(typeof(OrderUpdateReponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderItemStatus(long id, long orderItemId, [FromBody] OrderItemUpdateRequest status)
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
