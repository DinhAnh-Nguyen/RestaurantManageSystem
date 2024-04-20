using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.Components
{
    internal class Food
    {
        public String FoodName;
        public double Price;
        public String Description;
        public System.Drawing.Image foodImage;
        public Food(String foodname, double price, String description, System.Drawing.Image image) { 
            FoodName = foodname;
            Price = price;
            Description = description;
            foodImage = image;
        
        }
    }
}
