using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IOrderCommand
    {
        Task InsertOrder(domain.Entities.Order order);
        Task DeleteOrder(domain.Entities.Order order);
        Task UpdateOrder(domain.Entities.Order order);
    }
}
