using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.Components
{
    internal class Employee
    {
        public int EmployeeId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public int Age { get; set; }
        public String Position { get; set; }

        public Employee(int employeeid, String firstname, String lastname, String email, String phone, int age, String position)
        {
            EmployeeId = employeeid;
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            Phone = phone;
            Age = age;
            Position = position;
        }
        public Employee()
        {

        }
    }
}
