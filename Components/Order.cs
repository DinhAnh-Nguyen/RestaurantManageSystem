using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.Components
{
    internal class Order
    {
        public int OrderID { get; set; }
        public List<String> orderitems { get; set; }

        public Order() { 
        }
    }
}
