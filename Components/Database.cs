using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Maui.Controls;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using Microsoft.Maui.ApplicationModel.Communication;

namespace RestaurantManager.Components
{
    internal class Database
    { 
        //this creates the tables for our database
        // we have 3 tables: employees, food and foodorders
        public void CreateDB(string databaseName)
        {
            String DataBaseName = "Data Source=" + databaseName + ".db";
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

                    CREATE TABLE IF NOT EXISTS food(
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        cost REAL NOT NULL,
                        description TEXT NOT NULL,
                        photo BLOB NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS foodorder(
                        table_number INTEGER NOT NULL,
                        item TEXT NOT NULL
                    );
                ";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public void DeleteDB(string databaseName)
        {
            File.Delete(databaseName + ".db");
        }

        //this loads all existing employees into a list of type employee
        public List<employee> LoadDBEmployee(string databaseName)
        {

            String DataBaseName = "Data Source=" + databaseName + ".db";

            List<employee> Returnlist = new List<employee>();

            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    SELECT *
                    FROM employees
                ";
                //getting the values from the employee table to create the employee object
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetString(0);
                        var firstname = reader.GetString(1);
                        var lastname = reader.GetString(2);
                        var email = reader.GetString(3);
                        var phonenumber = reader.GetString(4);
                        int age = reader.GetInt32(5);

                        Returnlist.Add(new employee(firstname, lastname, email, phonenumber, age));
                    }
                }
                connection.Close();
            }
            return Returnlist;
        }

        // this returns a list of FOOD objects from the given database
        public List<Food> LoadDBFood(string databaseName)
        {
            List<Food> Returnlist = new List<Food>();

            String DataBaseName = "Data Source=" + databaseName + ".db";

            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    SELECT *
                    FROM Food
                ";

                //getting the values from the food table to create the food object
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetString(0);
                        var foodname = reader.GetString(1);
                        double price = Double.Parse(reader.GetString(2));
                        var description = reader.GetString(3);

                        byte[] imageBytes = (System.Byte[])reader[4];
                        var foodImage = ByteToImage(imageBytes);

                        Returnlist.Add(new Food(foodname, price, description, foodImage));
                        Debug.WriteLine($"Food Item info: {id}, {foodname}, {price}, {description}");
                    }
                }
                connection.Close();
            }
            return Returnlist;
        }

        //returns a dictionary of food item orders. the dictionary has the table as a key and a list of food items associated to it
        public Dictionary<int, List<string>> LoadDBOrders(string databaseName)
        {

            String DataBaseName = "Data Source=" + databaseName + ".db";

            Dictionary<int, List<string>> foodorders = new Dictionary<int, List<string>>();

            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    SELECT *
                    FROM foodorder
                ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int tablenum = reader.GetInt32(0);
                        var item = reader.GetString(1);

                        if (!foodorders.ContainsKey(tablenum)) 
                        {
                            foodorders[tablenum] = new List<string>();
                        }

                        foodorders[tablenum].Add(item);
 
                        Debug.WriteLine($"Order #{tablenum} item {item}");
                    }
                }
                connection.Close();
            }

            return foodorders;

        }

        //add food items to the database with photos
        public void AddFoodItem(String databaseName, String foodname, Double foodcost, String description, String image)
        {

            String DataBaseName = "Data Source=" + databaseName + ".db";

            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    INSERT INTO food(name, cost, description, photo)
                    VALUES ($name, $cost, $description, $photo)
                ";
                command.Parameters.AddWithValue("$name", foodname);
                command.Parameters.AddWithValue("$cost", foodcost);
                command.Parameters.AddWithValue("$description", description);
                command.Parameters.AddWithValue("$photo", ImageToByte(image, foodname));

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        //remove a fooditem from the table based on the food name
        public void RemoveFoodItem(String databaseName, String foodname)
        {
            String DataBaseName = "Data Source=" + databaseName + ".db";

            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    DELETE FROM food
                    WHERE name = $foodname
                ";
                command.Parameters.AddWithValue("$name", foodname);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        //adds employees to the table
        public void AddEmployee(String databaseName, String firstname, String lastname, String email, String phonenumber, int age)
        {

            String DataBaseName = "Data Source=" + databaseName + ".db";

            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    INSERT INTO employee(first_name, last_name, email, phone_number, age)
                    VALUES ($firstname, $lastname, $email, $phonenumber, $age)
                ";
                command.Parameters.AddWithValue("$first_name", firstname);
                command.Parameters.AddWithValue("$last_name", lastname);
                command.Parameters.AddWithValue("$email", email);
                command.Parameters.AddWithValue("$phonenumber", phonenumber);
                command.Parameters.AddWithValue("$age", age);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        //removes the employee from the table based on their first and last name
        public void RemoveEmployee(String databaseName, String firstname, String lastname)
        {
            String DataBaseName = "Data Source=" + databaseName + ".db";

            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    DELETE FROM employee
                    WHERE first_name = $firstname AND last_name = $lastname
                ";
                command.Parameters.AddWithValue("$first_name", firstname);
                command.Parameters.AddWithValue("$last_name", lastname);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        //add a food order to the database
        public void CreateOrder(String databaseName, List<String> items, int table)
        {
            String DataBaseName = "Data Source=" + databaseName + ".db";

            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                foreach (String item in items)
                {

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                    INSERT INTO foodorder(table_number, item)
                    VALUES ($tablenumber, $item)
                    ";
                    command.Parameters.AddWithValue("$item", item);
                    command.Parameters.AddWithValue("$tablenumber", table);

                    command.ExecuteNonQuery();

                }
                connection.Close();
            }
        }
        //removes a food order from the database, based on the table id
        public void CancelOrder(String databaseName, int table)
        {
            String DataBaseName = "Data Source=" + databaseName + ".db";

            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    DELETE FROM foodorder
                    WHERE table_number = $table
                ";
                command.Parameters.AddWithValue("$table", table);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        //turns images to bytes to be able to store in the database
        // found this here: https://stackoverflow.com/questions/10853301/save-and-load-image-sqlite-c-sharp
        public byte[] ImageToByte(String imageName, String fileName)
        {

            String imagePath = AppDomain.CurrentDomain.BaseDirectory;
            String actualPath = imagePath.Split("bin", StringSplitOptions.None)[0] + "Components/images/" + imageName;

            Debug.WriteLine(actualPath);

            System.Drawing.Image image = System.Drawing.Image.FromFile(actualPath);

            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();
                return imageBytes;
            }
        }
        //turns the bytes back into an image object
        // found this here: https://stackoverflow.com/questions/10853301/save-and-load-image-sqlite-c-sharp
        public System.Drawing.Image ByteToImage(byte[] imageBytes)
        {
            // Convert byte[] to Image
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = new Bitmap(ms);
            return image;
        }
    }
}
