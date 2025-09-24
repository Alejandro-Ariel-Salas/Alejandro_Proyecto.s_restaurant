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

        public async Task<List<Dish>> GetDishesByFilter(string? name, bool available, EnumSort sort, int? id)
        {
            var query = _context.dishes.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(d => d.Name.Contains(name));
            }

            if (available)
            {
                query = query.Where(d => d.Available == available);
            }

            if (available)
            {
                query = query.Where(d => d.Available == available);
            }

            if (sort == EnumSort.asc)
            {
                query = query.OrderBy(d => d.Price);
            }
            else
            {
                query = query.OrderByDescending(d => d.Price);
            }

            return await query.Include(d => d.Categorys).ToListAsync();
        }

        public async Task<bool> ExistCategory(int Id)
        {
            var exist = await _context.categories.AnyAsync(c => c.Id == Id);
            return exist;
        }
    }
}
