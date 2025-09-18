using Aplication.Models;
using Aplication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IOrderService
    {
        public Task<OrderResponse> CreateOrder(OrderModel order);
        public Task<List<OrderShowResponse>> GetOrderWithFilter(string? dateFrom, string? dateTo, int? status);
        public Task<OrderShowResponse> GetOrderById(long id);
        public Task<OrderUpdateResponse> UpdateItems(long id, OrderModifyModel orderModifyModel);
        public Task<OrderUpdateResponse> UpdateOrderItemStatus(long id, long orderItem, StatusModifyModel status);
    }
}
