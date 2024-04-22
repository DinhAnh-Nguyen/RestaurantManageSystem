using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.Components
{
    internal class Order
    {
        public int TableNumber { get; set; }
        public string CustomerName { get; set; }

        public Order(int table_number, string customer_name)
        { 
            TableNumber = table_number;
            CustomerName = customer_name;
        }

        public Order () { }
    }
}
