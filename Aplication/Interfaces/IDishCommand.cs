using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IDishCommand
    {
        Task InsertDish(Dish dish);
        Task DeleteDish(Dish dish);
        Task UpdateDish(Dish dish);
    }
}
