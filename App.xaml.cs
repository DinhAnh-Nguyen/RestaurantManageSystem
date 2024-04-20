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

            database.CreateDB("test4");
            database.AddFoodItem("test4","Sushi", 23.59, "Delicious platter of sushi", "Sushi.png");
            database.AddFoodItem("test4", "Sushi2", 23.59, "Delicious platter of sushi", "Sushi.png");
            database.LoadDBFood("test4");
            database.CreateOrder("test4", new List<string> {"sushi", "sushi2", "sushi"}, 50);
            database.LoadDBOrders("test4");
        }
    }
}
