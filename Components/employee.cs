using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.Components
{
    internal class employee
    {
        String FirstName;
        String Lastname;
        String Email;
        String Phone;
        int Age;

        public employee(String firstname, String lastname, String email, String phone, int age)
        {
            FirstName = firstname;
            Lastname = lastname;
            Email = email;
            Phone = phone;
            Age = age;
        }
    }
}
