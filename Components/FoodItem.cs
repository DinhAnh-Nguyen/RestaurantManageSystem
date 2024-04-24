using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.Components
{
    internal class FoodItem
    {
        // Properties to store information about the food item.

        // Unique identifier for the food item.
        public int Id { get; set; }
        // Name of the food item.
        public string Name { get; set; }
        // Cost of the food item.
        public double Cost { get; set; }
        // Description of the food item.
        public string Description { get; set; }
        // Photo of the food item.
        public System.Drawing.Image Photo { get; set; }

        // Constructor to initialize a FoodItem object with provided values.
        public FoodItem(int id, String foodname, double price, String description, System.Drawing.Image image)
        {
            // Assigning values to the properties.
            Id = id;
            Name = foodname;
            Cost = price;
            Description = description;
            Photo = image;
        }

        // Default constructor for FoodItem.
        public FoodItem() { }
    }
}
