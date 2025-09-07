using Aplication.Enums;
using Aplication.Exceptions;
using Aplication.Interfaces;
using Aplication.Models;
using Aplication.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Restaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpPost("Crear un Nuevo Plato")]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateDish([FromBody] DishModel dishModel)
        {
            try
            {
                var dish = await _dishService.CreateDish(dishModel);
                return CreatedAtAction(nameof(CreateDish), new { id = dish.DishId }, dish);
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

        [HttpGet("Buscar Platos")]
        [ProducesResponseType(typeof(List<DishResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchDishes( [FromQuery] string? name, [FromQuery] int? category, [FromQuery] EnumSort sort = EnumSort.asc, [FromQuery] bool available = true)
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

        [HttpPut("Actualizar un plato")]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateDish([FromQuery] Guid dish_Id, [FromBody] DishUpdateModel dish)
        {
            try
            {
                var updatedDish = await _dishService.UpdateDish(dish_Id, dish);
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
    }
}
