using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Responses
{
    public class OrderResponse
    {
        public long orderNumber { get; set; }
        public decimal totalAmount { get; set; }
        public DateTime createAt { get; set; }
    }

    public class OrderUpdateResponse
    {
        public long orderNumber { get; set; }
        public decimal totalAmount { get; set; }
        public DateTime updateAt { get; set; }
    }
}
