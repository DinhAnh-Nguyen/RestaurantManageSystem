﻿using Microsoft.Data.Sqlite;
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

            Database database = new Database("test4");

            database.CreateDB();
            database.AddFoodItem("Sushi", 23.59, "Delicious platter of sushi", "Sushi.png");
            database.AddFoodItem("Sushi2", 23.59, "Delicious platter of sushi", "Sushi.png");
            database.LoadDBFood();
            database.CreateOrder(new List<string> {"sushi", "sushi2", "sushi"}, 50);
            database.LoadDBOrders();

        }
    }
}
