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
        Task<List<Dish>> GetByCategoryId(int Id, bool available, EnumSort sort);
        Task<List<Dish>> GetDishesByName(string name, bool available, EnumSort sort);
        Task<bool> ExistCategory(int Id);
    }
}
