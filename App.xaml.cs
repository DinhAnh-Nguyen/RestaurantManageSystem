using Microsoft.Data.Sqlite;
using RestaurantManager.Components;
using System.Drawing;
using System.Reflection;

namespace RestaurantManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            Database database = new Database("test1");

            // database.AddFoodItem("Sushi", 23.59, "Delicious platter of sushi", "Sushi.png" as Byte[]);
            // database.RemoveFoodItem("Sushi");
            database.LoadDBFood();
        }
    }
}
