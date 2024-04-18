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

            // Load the image from file and convert it to a byte array
            byte[] imageBytes = LoadImageAsByteArray("/images/Sushi.png");


            // Add the food item with the byte array representation of the image
            database.AddFoodItem("Sushi", 23.59, "Delicious platter of sushi", imageBytes);
        }

        private static byte[] LoadImageAsByteArray(string imagePath)
        {
            // Assuming you have a method to load the image as a byte array
            // Implement this method according to your project's requirements
            // For example, you might use System.IO.File.ReadAllBytes(imagePath)
            // to read the image file and convert it to a byte array
            return null;
        }
    }
}
