using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Maui.Controls;
using System.Drawing;
using System.IO;

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
                    CREATE TABLE IF NOT EXISTS employees(
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        first_name TEXT NOT NULL,
                        last_name TEXT NOT NULL,
                        email TEXT NOT NULL,
                        phone_number TEXT NOT NULL,
                        age INTEGER NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS Food(
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        cost REAL NOT NULL,
                        description TEXT NOT NULL,
                        photo BLOB NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS  FoodOrder(
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
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

        public void LoadDB(string databaseName)
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

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var test = reader.GetString(0);

                        Console.WriteLine($"Test ourput {test}!");
                    }
                }
                connection.Close();
            }
        }

        public void AddFoodItem(String foodname, Double foodcost, String description, Image image)
        {

            byte[] imageBytes = (image); // Assume this converts ImageSource to byte array


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
        //AI assisted code below
        public byte[] ConvertImageToByteArray(Image image)
        {
            if (image == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                // Save the image to the MemoryStream in a specific format
                image.Save(ms, ImageFormat.Png); // You can change the format as needed (JPEG, BMP, etc.)
                return ms.ToArray();
            }
        }
}
