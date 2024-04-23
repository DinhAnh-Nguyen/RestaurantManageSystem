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

        public List<String> Items { get; set; }

        public Order(int table_number, string customer_name, List<String> orderitems)
        { 
            TableNumber = table_number;
            CustomerName = customer_name;
            Items = orderitems;
        }

        public Order () { }
    }
}
