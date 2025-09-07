using Aplication.Interfaces;
using domain.Entities;
using Infraesructure.Perssistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraesructure.Command
{
    public class DishCommand : IDishCommand
    {
        private readonly AppDBContext _context;

        public DishCommand(AppDBContext context)
        {
            _context = context;
        }

        public async Task DeleteDish(Dish dish)
        {
            _context.Remove(dish);
            await _context.SaveChangesAsync();

        }

        public async Task InsertDish(Dish dish)
        {
            _context.Add(dish);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDish(Dish dish)
        {
            _context.Update(dish);
            await _context.SaveChangesAsync();
        }
    }
}
