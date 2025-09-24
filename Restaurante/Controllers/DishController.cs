using Aplication.Enums;
using Aplication.Exceptions;
using Aplication.Interfaces;
using Aplication.Models;
using Aplication.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Numerics;
using System.Runtime.ConstrainedExecution;

namespace Restaurante.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }


        [HttpPost("Dish")]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateDish([FromBody] DishRequest dishModel)
        {
            try
            {
                var dish = await _dishService.CreateDish(dishModel);
                return CreatedAtAction(nameof(CreateDish), new { id = dish.Id }, dish);
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (ExceptionConflict ex)
            {
                return Conflict(new ApiError { Message = ex.Message });
            }
        }

        [HttpGet("Dish")]
        [ProducesResponseType(typeof(List<DishResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchDishes([FromQuery] string? name, [FromQuery] int? category, [FromQuery] EnumSort sort = EnumSort.asc, [FromQuery] bool available = true)
        {
            try
            {
                var dishes = await _dishService.GetDishes(name, category, sort, available);
                return Ok(dishes);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }

        [HttpGet("Dish/{Id}")]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(Guid Id)
        {
            try
            {
                var dish = await _dishService.GetDishById(Id);
                return Ok(dish);
            }
            catch (ExeptionNotFound ex)
            {
                return NotFound(new ApiError { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }

        [HttpPut("Dish/{id}")]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(Guid id, [FromBody] DishUpdateRequest dish)
        {
            try
            {
                var updatedDish = await _dishService.UpdateDish(id, dish);
                return Ok(updatedDish);
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (ExceptionConflict ex)
            {
                return Conflict(new ApiError { Message = ex.Message });
            }
        }

        [HttpDelete("Dish/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleteDish = await _dishService.DeleteDish(id);
                return Ok(deleteDish);
            }
            catch (ExeptionNotFound ex)
            {
                return NotFound(new ApiError { Message = ex.Message });
            }
            catch (ExceptionConflict ex)
            {
                return Conflict(new ApiError { Message = ex.Message });
            }
        }

        [HttpGet("Category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategorys()
        {
            var categorys = await _dishService.GetAllCategory();
            return Ok(categorys);
        }

        [HttpGet("DeliveryType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDeliveryTypes()
        {
            var deliveryTypes = await _dishService.GetAllDeliveryType();
            return Ok(deliveryTypes);
        }

        [HttpGet("Status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStatus()
        {
            var status = await _dishService.GetAllStatus();
            return Ok(status);
        }
    }
}
