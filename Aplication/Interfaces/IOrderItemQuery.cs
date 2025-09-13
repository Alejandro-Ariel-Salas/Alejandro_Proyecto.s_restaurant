using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IOrderItemQuery
    {
        Task<List<OrderItem>> GetOrderItemsByDishId(Guid dishId);
        Task<List<OrderItem>> GetOrderItemsByOrderId(long orderId); 
    }
}
