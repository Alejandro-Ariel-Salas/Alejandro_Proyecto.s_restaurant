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
    public class OrderItemQuery : IOrderItemQuery
    {
        private readonly AppDBContext _context;
        public OrderItemQuery( AppDBContext context) 
        { 
            _context = context;
        }

        public async Task<List<OrderItem>> GetOrderItemsByDishId(Guid dishId)
        {
            var orderItems = await _context.orderItems.Where(oi => oi.Dish == dishId && oi.Orders.OverallStatus != 5).Include(oi => oi.Order).ToListAsync();
            return orderItems;
        }
    }
}
