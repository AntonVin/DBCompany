using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCompany
{
    public class Employee
    {
        public int? Id {get;}
        public string LastName { get; }
        public string? FirstName { get; }
        public string? Patronymic { get; }
        public string? Position { get; }
        public string Login { get; }
        public string Password { get; }

        public Employee(int? id,string lastName, string? firstName, string? patronymic, string? position, string login, string password)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            Patronymic = patronymic;
            Position = position;
            Login = login;
            Password = password;
        }
    }
}
