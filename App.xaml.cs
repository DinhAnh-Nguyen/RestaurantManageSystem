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
           
        }
    }
}
