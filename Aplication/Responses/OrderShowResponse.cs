using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Responses
{
    public class OrderShowResponse
    {
        public long orderNumber { get; set; }
        public decimal totalAmount { get; set; }
        public string deliveryTo { get; set; }
        public string notes { get; set; }
        public StatusResponse status { get; set; }
        public DeliveryTypeResponse deliveryType { get; set; }
        public List<OrderItemShowResponse> items { get; set; }
    }

    public class OrderItemShowResponse
    {
        public long id { get; set; }
        public int quantity { get; set; }
        public string notes { get; set; }
        public StatusResponse status { get; set; }
        public DishShowResponse dish { get; set; }

    }

    public class DishShowResponse
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
    }
}
