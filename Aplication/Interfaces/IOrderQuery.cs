using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IOrderQuery
    {
        Task<Order> GetOrderById(long id);
        Task<List<Order>> GetOrders(DateTime? dateFrom, DateTime? dateTo, int? status);
        Task<Order> GetOrderByItemId(long id);
    }
}
