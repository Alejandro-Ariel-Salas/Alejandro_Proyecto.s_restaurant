using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Models
{
    public class OrderModel
    {
        public List<ItemModel> items { get; set; }
        public DeliveryModel delivery { get; set; }
        public string notes { get; set; }
    }

    public class ItemModel
    {
        public Guid id { get; set; }
        public int quantity { get; set; }
        public string notes { get; set; }
    }

    public class DeliveryModel
    {
        public int id { get; set; }
        public string to { get; set; }
    }
}
