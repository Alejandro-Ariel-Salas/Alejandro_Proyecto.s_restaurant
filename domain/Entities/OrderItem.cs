using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }   
        public long Order { get; set; }
        public Order Orders { get; set; }
        public Guid Dish { get; set; }
        public Dish Dishes { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
        public int Status { get; set; }
        public Status Statuses { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
