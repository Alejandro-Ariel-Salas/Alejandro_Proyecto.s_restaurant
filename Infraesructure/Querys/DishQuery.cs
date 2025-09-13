using Aplication.Enums;
using Aplication.Interfaces;
using domain.Entities;
using Infraesructure.Perssistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraesructure.Querys
{
    public class DishQuery : IDishQuery
    {
        private readonly AppDBContext _context;

        public DishQuery(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<Dish>> GetAllDishes()
        {
            var dishes = await _context.dishes.ToListAsync();
            return dishes;

        }

        public async Task<List<Dish>> GetByCategoryId(int Id, bool available, EnumSort sort)
        {
            if (sort == EnumSort.asc)
            {
                var dishes = await _context.dishes.Where(d => d.Category == Id && d.Available == available).Include(d => d.Categorys).OrderBy(d => d.Price).ToListAsync();
                return dishes;
            }
            else
            {
                var dishes = await _context.dishes.Where(d => d.Category == Id && d.Available == available).Include(d => d.Categorys).OrderByDescending(d => d.Price).ToListAsync();
                return dishes;
            }
        }

        public async Task<Dish> GetDishById(Guid DishId)
        {
            var dish = await _context.dishes.Include(d => d.Categorys).FirstOrDefaultAsync(d => d.DishId == DishId);
            return dish;
        }

        public async Task<Dish> GetDishByName(string name)
        {
            var dish = await _context.dishes.Include(d => d.Categorys).FirstOrDefaultAsync(d => d.Name == name);
            return dish;
        }

        public async Task<List<Dish>> GetDishesByName(string name, bool available, EnumSort sort)
        {
            if (sort == EnumSort.asc)
            {
                var dishes = await _context.dishes.Where(d => d.Name.ToLower().Contains(name.ToLower()) && d.Available == available).Include(d => d.Categorys).OrderBy(d => d.Price).ToListAsync();
                return dishes;
            }
            else
            {
                var dishes = await _context.dishes.Where(d => d.Name.ToLower().Contains(name.ToLower()) && d.Available == available).Include(d => d.Categorys).OrderByDescending(d => d.Price).ToListAsync();
                return dishes;
            }
        }

        public async Task<bool> ExistCategory(int Id)
        {
            var exist = await _context.categories.AnyAsync(c => c.Id == Id);
            return exist;
        }
    }
}
