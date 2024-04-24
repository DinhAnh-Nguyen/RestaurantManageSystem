using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.Components
{
    internal class Order
    {
        // Properties to store information about the order.

        // Table number where the order is placed.
        public int TableNumber { get; set; }
        // Name of the customer who placed the order.
        public string CustomerName { get; set; }

        // List of items included in the order.
        public List<String> Items { get; set; }

        // Constructor to initialize an Order object with provided values.
        public Order(int table_number, string customer_name, List<String> orderitems)
        {
            // Assigning values to the properties.
            TableNumber = table_number;
            CustomerName = customer_name;
            Items = orderitems;
        }
        // Default constructor for Order.
        public Order() { }
    }
}
