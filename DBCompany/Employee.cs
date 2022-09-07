using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCompany
{
    public class Employee
    {
        public int Id {get;}
        public string LastName { get; }
        public string? FirstName { get; }
        public string? Patronymic { get; }
        public string? Position { get; }
        public string Login { get; }
        public string Password { get; }

        public Employee(string lastName, string? firstName, string? patronymic, string? position, string login, string password)
        {
            Id = -1;
            LastName = lastName;
            FirstName = firstName;
            Patronymic = patronymic;
            Position = position;
            Login = login;
            Password = password;
        }
        public Employee(int id,string lastName, string? firstName, string? patronymic, string? position, string login, string password)
            :this(lastName,firstName,patronymic,position,login,password)
        {
            this.Id = id;
        }
    }
}
