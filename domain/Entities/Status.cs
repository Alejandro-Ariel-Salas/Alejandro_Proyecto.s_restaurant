using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    public class Status
    {
        public int StatusId { get; set; }
        public string Name { get; set; }
        public IList<Order> Orders { get; set; }
        public IList<OrderItem> OrderItems { get; set; }
    }
}
