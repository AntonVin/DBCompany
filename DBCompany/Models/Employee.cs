using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCompany
{
    public class Employee
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Patronymic { get; set; }
        public string? Position { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

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
