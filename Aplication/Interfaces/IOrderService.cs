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
        public Task<OrderCreateReponse> CreateOrder(OrderRequest order);
        public Task<List<OrderDetailResponse>> GetOrderWithFilter(string? dateFrom, string? dateTo, int? status);
        public Task<OrderDetailResponse> GetOrderById(long id);
        public Task<OrderUpdateReponse> UpdateItems(long id, OrderUpdateRequest orderModifyModel);
        public Task<OrderUpdateReponse> UpdateOrderItemStatus(long id, long orderItem, OrderItemUpdateRequest status);
    }
}
