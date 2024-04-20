using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.Components
{
    internal class FoodItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; }
        public System.Drawing.Image Photo { get; set; }

        public FoodItem(int id, String foodname, double price, String description, System.Drawing.Image image)
        {
            Id = id;
            Name = foodname;
            Cost = price;
            Description = description;
            Photo = image;
        }

        public FoodItem() { }
    }
}
