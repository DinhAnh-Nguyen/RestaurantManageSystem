using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.Components
{
    // The Employee class represents an employee working at the restaurant.
    internal class Employee
    {
        // Properties to store information about the employee.

        // Unique identifier for the employee.
        public int EmployeeId { get; set; }
        // First name of the employee.
        public String FirstName { get; set; }
        // Last name of the employee.
        public String LastName { get; set; }
        // Email address of the employee.
        public String Email { get; set; }
        // Phone number of the employee.
        public String Phone { get; set; }
        // Age of the employee.
        public int Age { get; set; }

        // Position or role of the employee.
        public String Position { get; set; }

        // Constructor to initialize an Employee object with provided values.
        public Employee(int employeeid, String firstname, String lastname, String email, String phone, int age, String position)
        {
            // Assigning values to the properties.
            EmployeeId = employeeid;
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            Phone = phone;
            Age = age;
            Position = position;
        }

        // Default constructor for Employee.
        public Employee()
        {

        }
    }
}
