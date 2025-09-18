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
    public class OrderItemCommand : IOrderItemCommand
    {
        private readonly AppDBContext _context;

        public OrderItemCommand(AppDBContext context)
        {
            _context = context;
        }

        public async Task DeleteOrderItem(OrderItem orderItem)
        {
            _context.Remove(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task InsertOrderItem(OrderItem orderItem)
        {
            _context.Add(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderItem(OrderItem orderItem)
        {
            _context.Update(orderItem);
            await _context.SaveChangesAsync();
        }
    }
}
