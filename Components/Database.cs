using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;       
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace RestaurantManager.Components
{
    internal class Database
    {
        private String _databaseName;

        public void CreateDB(string databaseName)
        {
            _databaseName = $"Data Source={databaseName}.db";
            using (var connection = new SqliteConnection(_databaseName))
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
            }
        }

        public void DeleteDB(string databaseName)
        {

        }

        public void LoadDB(string databaseName)
        {
            using (var connection = new SqliteConnection($"Data Source={databaseName}.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM employees";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var test = reader.GetString(0);
                        Console.WriteLine($"Test output: {test}!");
                    }
                }
            }
        }

        public void AddFoodItem(string foodname, double foodcost, string description, byte[] imageBytes)
        {
            try
            {
                using (var connection = new SqliteConnection(_databaseName))
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
                    command.Parameters.AddWithValue("$photo", imageBytes);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or log it for debugging
                Console.WriteLine("Error occurred while adding food item: " + ex.Message);
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
        public byte[] ConvertImageToByteArray(Microsoft.Maui.Controls.Image image)
        {
            if (image == null || image.Source == null)
                return null;

            // Convert ImageSource to SixLabors.ImageSharp.Image
            SixLabors.ImageSharp.Image<Rgba32> sharpImage = null;

            // Assuming you're using a file-based ImageSource (e.g., FileImageSource)
            if (image.Source is FileImageSource fileImageSource)
            {
                // Load the image from file
                sharpImage = (Image<Rgba32>?)SixLabors.ImageSharp.Image.Load(fileImageSource.File);
            }
            // You may need to handle other types of ImageSource here (e.g., StreamImageSource)

            if (sharpImage == null)
                return null;

            using (MemoryStream stream = new MemoryStream())
            {
                // Save SixLabors.ImageSharp.Image to MemoryStream as PNG
                sharpImage.Save(stream, new PngEncoder());
                return stream.ToArray();
            }
        }
    }
}
