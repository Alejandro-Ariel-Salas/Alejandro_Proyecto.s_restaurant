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
    public class CategoryQuery : ICategoryQuery
    {
         private readonly AppDBContext _context;

        public CategoryQuery(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            var categories = await _context.categories.ToListAsync();
            return categories;
        }
    }
}
