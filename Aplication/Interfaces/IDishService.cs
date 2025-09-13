using Aplication.Enums;
using Aplication.Models;
using Aplication.Responses;
using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IDishService
    {
        Task<DishResponse> CreateDish(DishModel createDishRequest);
        Task<DishResponse> DeleteDish(Guid id);
        Task<DishResponse> UpdateDish(Guid dishId,DishUpdateModel dish);
        Task<List<DishResponse>> GetAllDishes();
        Task<DishResponse> GetDishById(Guid id);
        Task DishValidation(DishModel dish);
        Task<List<DishResponse>> GetDishes(string? name, int? category, EnumSort sort, bool dishAvailable);
        Task<List<CategorysResponse>> GetAllCategory();
        Task<List<DeliveryTypeResponse>> GetAllDeliveryType();
        Task<List<StatusResponse>> GetAllStatus();
    }
}
