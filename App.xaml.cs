using Microsoft.Data.Sqlite;
using RestaurantManager.Components;

namespace RestaurantManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            Database database = new Database();

            database.CreateDB("test1");
            database.AddFoodItem("Sushi", 23.59, "Delicious platter of sushi", new Image { Source = "/images/Sushi.png" });
        }
    }
}
