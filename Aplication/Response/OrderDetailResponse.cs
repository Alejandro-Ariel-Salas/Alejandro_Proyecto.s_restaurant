using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Responses
{
    public class OrderDetailResponse
    {
        public long OrderNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string DeliveryTo { get; set; }
        public string Notes { get; set; }
        public StatusResponse Status { get; set; }
        public DeliveryTypeResponse DeliveryType { get; set; }
        public List<OrderItemShowResponse> Items { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class OrderItemShowResponse
    {
        public long id { get; set; }
        public int quantity { get; set; }
        public string notes { get; set; }
        public StatusResponse status { get; set; }
        public DishShortResponse dish { get; set; }

    }

    public class DishShortResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
