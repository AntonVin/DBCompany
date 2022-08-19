using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DBCompany
{
    public class ManipulatorTableDB
    {
        string connectionString;
        string table;

        public ManipulatorTableDB(string connectionString, string table)
        {
            this.connectionString = connectionString;
            this.table = table;
        }

        public List<Employee> GetDataList()
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            var outputList = new List<Employee>();
            string stringExpression = $"SELECT * FROM {this.table}";

            var command = new SqlCommand(stringExpression, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string lastName = reader["LastName"].ToString();
                    string firstName = reader["FirstName"].ToString();
                    string patronymic = reader["Patronymic"].ToString();
                    string position = reader["Position"].ToString();
                    string login = reader["Login"].ToString();
                    string password = reader["Password"].ToString();

                    outputList.Add(new Employee(id, lastName, firstName, patronymic, position, login, password));
                }
            }
            return outputList;
        }
        public Employee? GetEmployeeById(int id)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            string stringExpression = $"SELECT * FROM {this.table} WHERE Id={id}";
            var command = new SqlCommand(stringExpression, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read() == false) return null;
                string lastName = reader["LastName"].ToString();
                string firstName = reader["FirstName"].ToString();
                string patronymic = reader["Patronymic"].ToString();
                string position = reader["Position"].ToString();
                string login = reader["Login"].ToString();
                string password = reader["Password"].ToString();
                return new Employee(id, lastName, firstName, patronymic, position, login, password);
            }
        }
        public int InsertRow(Employee newEmployee)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            string stringExpesstion = $"INSERT {this.table} VALUES (@LastName, @FirstName, @Patronymic, @Position, @Login, @Password)";

            var command = new SqlCommand(stringExpesstion, connection);
            SqlParameter lastNameParam = new SqlParameter("@LastName", newEmployee.LastName);
            SqlParameter firstNameParam = new SqlParameter("@FirstName", newEmployee.FirstName);
            SqlParameter patronymicParam = new SqlParameter("@Patronymic", newEmployee.Patronymic);
            SqlParameter positionParam = new SqlParameter("@Position", newEmployee.Position);
            SqlParameter loginParam = new SqlParameter("@Login", newEmployee.Login);
            SqlParameter passwordParam = new SqlParameter("@Password", newEmployee.Password);
            command.Parameters.AddRange(new SqlParameter[] { firstNameParam, lastNameParam, patronymicParam, positionParam, loginParam, passwordParam });

            command.ExecuteNonQuery();
            command.CommandText = "SELECT @@IDENTITY";
            using SqlDataReader reader = command.ExecuteReader();
            if (reader.Read() == false) 
                return -1;
            return Convert.ToInt32(reader[0]);
        }
        public bool ChangeRow(Employee changedEmployee)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();
            string stringExpression = $"SELECT Id FROM {this.table}";
            var command = new SqlCommand(stringExpression, connection);
            using var reader = command.ExecuteReader();
            {
                if (reader.HasRows == false)
                    return false;
                reader.Close();
            }

            command.CommandText = $"UPDATE {this.table} SET LastName=@LastName, FirstName=@FirstName, Patronymic=@Patronymic," +
                                      $"Position = @Position, Login = @Login, Password=@Password WHERE Id={changedEmployee.Id}";
            SqlParameter lastNameParam = new SqlParameter("@LastName", changedEmployee.LastName);
            SqlParameter firstNameParam = new SqlParameter("@FirstName", changedEmployee.FirstName);
            SqlParameter patronymicParam = new SqlParameter("@Patronymic", changedEmployee.Patronymic);
            SqlParameter positionParam = new SqlParameter("@Position", changedEmployee.Position);
            SqlParameter loginParam = new SqlParameter("@Login", changedEmployee.Login);
            SqlParameter passwordParam = new SqlParameter("@Password", changedEmployee.Password);
            command.Parameters.AddRange(new SqlParameter[] { firstNameParam, lastNameParam, patronymicParam, positionParam, loginParam, passwordParam });

            command.ExecuteNonQuery();
            return true;
        }
        public bool DeleteRow(int id)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            string stringExpesstion = $"SELECT * FROM {this.table} WHERE Id={id}";
            var command = new SqlCommand(stringExpesstion, connection);
            using SqlDataReader reader = command.ExecuteReader();
            {
                if (reader.HasRows == false)
                    return false;
                reader.Close();
            }
            command.CommandText = $"DELETE FROM {this.table} WHERE Id={id}";
            command.ExecuteNonQuery();
            return true;
        }
    }
}
