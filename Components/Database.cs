using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Maui.Controls;
using static Java.Util.Jar.Attributes;

namespace RestaurantManager.Components
{
    internal class Database
    {

        public String DataBaseName { get; set; }

        public void CreateDB(string databaseName)
        {
            DataBaseName = "Data Source=" + databaseName + ".db";
            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    CREATE TABLE employees(
                        id INTEGER NOT NULL AUTOINCREMENT,
                        first_name TEXT NOT NULL,
                        last_name TEXT NOT NULL,
                        email TEXT NOT NULL,
                        phone_number TEXT NOT NULL,
                        age INTEGER NOT NULL
                    );

                    CREATE TABLE Food(
                        id INTEGER NOT NULL AUTOINCREMENT,
                        name TEXT NOT NULL,
                        cost REAL NOT NULL,
                        description TEXT NOT NULL,
                        photo BLOB NOT NULL
                    );

                    CREATE TABLE Order(
                        id INTEGER NOT NULL AUTOINCREMENT,
                        item TEXT NOT NULL
                    );

                    
                ";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public void DeleteDB(string databaseName)
        {

        }

        public List<Object> LoadDB(string databaseName)
        {
            List<Object> Returnlist = new List<Object>();

            using (var connection = new SqliteConnection(databaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText = 
                @"
                    SELECT *
                    FROM employees
                    
                ";

            }

        public void AddFoodItem(String foodname, Double foodcost, String description, Image image)
        {
            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    INSERT INTO food (name, cost, description, photo)
                    VALUES ($name, $cost, $description, $photo)
                ";
                command.Parameters.AddWithValue("$name", foodname);
                command.Parameters.AddWithValue("$cost", foodcost);
                command.Parameters.AddWithValue("$description", description);
                command.Parameters.AddWithValue("$photo", image);
                
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public void RemoveFoodItem(String foodname)
        {

        }
        public void AddEmployee(String firstname, String lastname, String email, String phonenumber, int age)
        {

        }
        public void RemoveEmployee(String firstname, String lastname)
        {

        }
        public void CreateOrder(List<String> items, DateTime timeplaced)
        {

        }
        public void CancelOrder(int orderid)
        {

        }
    }
}
