using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Models
{
    public class OrderRequest
    {
        public List<Items> Items { get; set; }
        public Delivery Delivery { get; set; }
        public string Notes { get; set; }
    }

    public class Items
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
    }

    public class Delivery
    {
        public int Id { get; set; }
        public string To { get; set; }
    }
}
