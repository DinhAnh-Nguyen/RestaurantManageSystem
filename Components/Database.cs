﻿using System;
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

//Group 7
//Names: Abdurahman, Anamika, Dinh Anh, Max
//Project Description: This is our Restaraunt Manager application. 
//In this application we allow the user to add employees, food items, and food orders in a restaraunt

namespace RestaurantManager.Components
{
    internal class Database
    {

        String databaseName;

        public bool IsInfoCovered { get; set; }

        public Database(String dbName)
        {
            databaseName = dbName;
        }

        // this creates the tables for our database
        // we have 3 tables: employees, food and foodorders
        public void CreateDB()
        {
            String DataBaseName = "Data Source=" + databaseName + ".db";
            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS employee(
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        first_name TEXT NOT NULL,
                        last_name TEXT NOT NULL,
                        email TEXT NOT NULL,
                        phone_number TEXT NOT NULL,
                        age INTEGER NOT NULL,
                        position TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS food(
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        cost REAL NOT NULL,
                        description TEXT NOT NULL,
                        photo BLOB
                    );

                    CREATE TABLE IF NOT EXISTS foodorder(
                        table_number INTEGER NOT NULL,
                        customer_name TEXT NOT NULL,
                        food_items TEXT
                    );
                ";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public void DeleteDB()
        {
            File.Delete(databaseName + ".db");
        }

        // this loads all existing employees into a list of type employee
        public List<Employee> LoadDBEmployee()
        {

            String DataBaseName = "Data Source=" + databaseName + ".db";

            List<Employee> Returnlist = new List<Employee>();

            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    SELECT *
                    FROM employee
                ";
                // getting the values from the employee table to create the employee object
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        var firstname = reader.GetString(1);
                        var lastname = reader.GetString(2);
                        var email = reader.GetString(3);
                        var phonenumber = reader.GetString(4);
                        int age = reader.GetInt32(5);
                        var position = reader.GetString(6);

                        Returnlist.Add(new Employee(id, firstname, lastname, email, phonenumber, age, position));
                    }
                }
                connection.Close();
            }
            return Returnlist;
        }

        // this returns a list of FOOD objects from the given database
        public List<FoodItem> LoadDBFood()
        {
            List<FoodItem> Returnlist = new List<FoodItem>();

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

                // getting the values from the food table to create the food object
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var foodname = reader.GetString(1);
                        double price = Double.Parse(reader.GetString(2));
                        var description = reader.GetString(3);

                        String imagePath = AppDomain.CurrentDomain.BaseDirectory;
                        String actualPath = imagePath.Split("bin", StringSplitOptions.None)[0] + "Components/images/" + "Sushi.png";
                        System.Drawing.Image image = System.Drawing.Image.FromFile(actualPath);
                        var foodImage = image;

                        try
                        {
                            byte[] imageBytes = (System.Byte[])reader[4];
                            foodImage = ByteToImage(imageBytes);
                        }
                        catch(System.InvalidCastException ex)
                        {

                        }

                        Returnlist.Add(new FoodItem(id, foodname, price, description, foodImage));
                        Debug.WriteLine($"Food Item info: {id}, {foodname}, {price}, {description}");
                    }
                }
                connection.Close();
            }
            return Returnlist;
        }

        // returns a dictionary of food item orders. the dictionary has the table as a key and a list of food items associated to it
        // a bit messy because of being able to add multiple order items to the same table entry
        // using a dictionary with a touple as a key so we store the table number and order number there
        // then using a list of Strings we add the food items to that associated touple
        public List<Order> LoadDBOrders()
        {
            List<Order> Returnlist = new List<Order>();

            Dictionary<(int, String), List<String>> tableOrders = new Dictionary<(int, String), List<String>>();

            String DataBaseName = "Data Source=" + databaseName + ".db";

            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    SELECT table_number, customer_name
                    FROM foodorder
                ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int table_number = reader.GetInt32(0);
                        String customer_name = reader.GetString(1);

                        using (var itemConnection = new SqliteConnection(DataBaseName))
                        {
                            itemConnection.Open();

                            var itemCommand = itemConnection.CreateCommand();

                            itemCommand.CommandText =
                            @"
                                SELECT food_items
                                FROM foodorder
                                WHERE table_number = $tablenumber
                            ";
                            itemCommand.Parameters.AddWithValue("$tablenumber", table_number);

                            List<string> items = new List<string>();

                            using (var itemReader = itemCommand.ExecuteReader())
                            {
                                while (itemReader.Read())
                                {
                                    if (tableOrders.ContainsKey((table_number, customer_name)))
                                    {
                                        //making sure there are no duplicate entries
                                        if (!tableOrders[(table_number, customer_name)].Contains(itemReader.GetString(0)))
                                            tableOrders[(table_number, customer_name)].Add(itemReader.GetString(0));
                                    }
                                    else
                                    {
                                        tableOrders.Add((table_number, customer_name), new List<string> { itemReader.GetString(0) });
                                    }
                                }
                            }
                            Debug.WriteLine($"Order info: {table_number}, {customer_name}");
                        }
                    }
                }
                connection.Close();
            }
            //this is the magic that makes it all work. turning the dictionary into order objects
            foreach (var key in tableOrders.Keys)
            {
                Returnlist.Add(new Order(key.Item1, key.Item2, tableOrders[key]));
            }

            return Returnlist;
        }

        // add food items to the database with photos
        public void AddFoodItem(String foodname, Double foodcost, String description, String image)
        {

            String DataBaseName = "Data Source=" + databaseName + ".db";
            IsInfoCovered = false;

            try
            {
                using (var connection = new SqliteConnection(DataBaseName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                    INSERT INTO food(name, cost, description)
                    VALUES ($name, $cost, $description)
                    ";
                    command.Parameters.AddWithValue("$name", foodname);
                    command.Parameters.AddWithValue("$cost", foodcost);
                    command.Parameters.AddWithValue("$description", description);
                    //command.Parameters.AddWithValue("$photo", ImageToByte(image, foodname));

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                IsInfoCovered = true;
            }
        }

        public void ModifyFoodItem(int id, string name, double cost, string description, String image)
        {
            String DataBaseName = "Data Source=" + databaseName + ".db";

            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE food SET name = $name, cost = $cost, description = $description WHERE id = $id";
                command.Parameters.AddWithValue("$id", id);
                command.Parameters.AddWithValue("$name", name);
                command.Parameters.AddWithValue("$cost", cost);
                command.Parameters.AddWithValue("$description", description);
                //command.Parameters.AddWithValue("$photo", ImageToByte(image, name));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        // remove a fooditem from the table based on the food name
        public void RemoveFoodItem(int id)
        {
            String DataBaseName = "Data Source=" + databaseName + ".db";

            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    DELETE FROM food
                    WHERE id = $id
                ";
                command.Parameters.AddWithValue("$id", id);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        // adds employees to the table
        public void AddEmployee(String firstname, String lastname, String email, String phonenumber, int age, String position)
        {

            String DataBaseName = "Data Source=" + databaseName + ".db";
            IsInfoCovered = false;

            try
            {
                using (var connection = new SqliteConnection(DataBaseName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                    INSERT INTO employee(first_name, last_name, email, phone_number, age, position)
                    VALUES ($firstname, $lastname, $email, $phonenumber, $age, $position)
                ";
                    command.Parameters.AddWithValue("$firstname", firstname);
                    command.Parameters.AddWithValue("$lastname", lastname);
                    command.Parameters.AddWithValue("$email", email);
                    command.Parameters.AddWithValue("$phonenumber", phonenumber);
                    command.Parameters.AddWithValue("$age", age);
                    command.Parameters.AddWithValue("$position", position);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                IsInfoCovered = true;
            }
        }
        public void ModifyEmployee(int id, String firstname, String lastname, String email, String phone, int age, String position)
        {
            String DataBaseName = "Data Source=" + databaseName + ".db";
            try {
                using (var connection = new SqliteConnection(DataBaseName))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "UPDATE employee SET first_name = $firstname, last_name = $lastname, email = $email, phone_number = $phone, age = $age, position = $position WHERE id = $id";
                    command.Parameters.AddWithValue("$id", id);
                    command.Parameters.AddWithValue("$firstname", firstname);
                    command.Parameters.AddWithValue("$lastname", lastname);
                    command.Parameters.AddWithValue("$email", email);
                    command.Parameters.AddWithValue("$phone", phone);
                    command.Parameters.AddWithValue("$age", age);
                    command.Parameters.AddWithValue("$position", position);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                IsInfoCovered = true;
            }
        }

        // removes the employee from the table based on their first and last name
        public void RemoveEmployee(int id)
        {
            String DataBaseName = "Data Source=" + databaseName + ".db";

            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    DELETE FROM employee
                    WHERE id = $id
                ";
                command.Parameters.AddWithValue("$id", id);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        // add a food order to the database
        public void CreateOrder(int table_number, string customer_name, List<String> items)
        {
            String DataBaseName = "Data Source=" + databaseName + ".db";
            IsInfoCovered = false;

            if (customer_name == null)
            {
                IsInfoCovered = true;
            }

            try
            {
                using (var connection = new SqliteConnection(DataBaseName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    foreach (var item in items)
                    {
                        command.Parameters.Clear();
                        command.CommandText =
                        @"
                        INSERT INTO foodorder(table_number, customer_name, food_items)
                        VALUES ($tablenumber, $customername, $item)
                        ";
                        command.Parameters.AddWithValue("$tablenumber", table_number);
                        command.Parameters.AddWithValue("$customername", customer_name);
                        command.Parameters.AddWithValue("$item", item);

                        command.ExecuteNonQuery();

                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                IsInfoCovered = true;
            }
            
        }

        // removes a food order from the database, based on the table id
        public void CancelOrder(int table_number)
        {
            String DataBaseName = "Data Source=" + databaseName + ".db";

            using (var connection = new SqliteConnection(DataBaseName))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    DELETE FROM foodorder
                    WHERE table_number = $tablenumber
                ";
                command.Parameters.AddWithValue("$tablenumber", table_number);

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

            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(actualPath);

                using (MemoryStream ms = new MemoryStream())
                {
                    // Convert Image to byte[]
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageBytes = ms.ToArray();
                    return imageBytes;
                }
            }
            catch (System.IO.FileNotFoundException ex) { 
            
            }

            return null;
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
