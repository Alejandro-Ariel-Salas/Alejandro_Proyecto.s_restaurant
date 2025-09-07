using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    public class Order
    {
        public long OrderId { get; set; }
        public int DeliveryTypeId { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public string DeliveryTo { get; set; }
        public int OverallStatus { get; set; }
        public Status Status { get; set; }
        public string Notes { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }
}
