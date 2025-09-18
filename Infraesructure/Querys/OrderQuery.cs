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
    public class OrderQuery : IOrderQuery
    {
        private readonly AppDBContext _context;

        public OrderQuery(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderById(long id)
        {
            var order = await _context.orders
                .Include(o => o.DeliveryTypes)
                .Include(o => o.Status)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Dishes)
                .FirstOrDefaultAsync(o => o.OrderId == id);
            return order;
        }

        public async Task<Order> GetOrderByItemId(long id)
        {
            var orderItem = await _context.orders.Include(o => o.OrderItems).FirstOrDefaultAsync(oi => oi.OrderId == id);
            return orderItem;
        }

        public Task<List<Order>> GetOrders(DateTime? dateFrom, DateTime? dateTo, int? status)
        {
            var orders = _context.orders
                .Include(o => o.DeliveryTypes)
                .Include(o => o.Status)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Dishes)
                .AsQueryable();

            if (dateFrom.HasValue && dateTo.HasValue)
            {
                orders = orders.Where(o => o.CreateDate >= dateFrom && o.CreateDate <= dateTo);
            }
            if (status.HasValue && status > 0)
            {
                orders = orders.Where(o => o.OverallStatus == status);
            }

            return orders.ToListAsync();
        }
    }
}
