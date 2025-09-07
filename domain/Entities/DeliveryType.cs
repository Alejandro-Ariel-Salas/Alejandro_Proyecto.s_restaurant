using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    public class DeliveryType
    {
        public int DeliveryTypeId { get; set;}
        public string Name {get; set;}

        public IList<Order> Orders { get; set; }
    }
}
