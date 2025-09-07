using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    public class Category
    {
        public int CategoryId { get; set;}
        public string Name { get; set;}
        public string Description { get; set;}
        public int Order { get; set; }
        public IList<Dish> Dishes { get; set; }
    }
}
