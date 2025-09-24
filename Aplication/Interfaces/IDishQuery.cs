using Aplication.Enums;
using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IDishQuery
    {
        Task<List<Dish>> GetAllDishes();
        Task<Dish> GetDishById(Guid DishId);
        Task<Dish> GetDishByName(string name);
        Task<List<Dish>> GetDishesByFilter(string? name, bool available, EnumSort sort, int? id);
        Task<bool> ExistCategory(int Id);
    }
}
