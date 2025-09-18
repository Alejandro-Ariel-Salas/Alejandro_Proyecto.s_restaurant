using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IOrderItemCommand
    {
        Task InsertOrderItem(domain.Entities.OrderItem orderItem);
        Task DeleteOrderItem(domain.Entities.OrderItem orderItem);
        Task UpdateOrderItem(domain.Entities.OrderItem orderItem);
    }
}
