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
        SqlConnection connection;
        string table;

        public ManipulatorTableDB(SqlConnection connection, string table)
        {
            this.connection = connection;
            this.table = table;
        }

        public List<Employee> GetDataList()
        {
            var outputList = new List<Employee>();
            string stringExpression = $"SELECT * FROM {this.table}";

            var command = new SqlCommand(stringExpression, this.connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string lastName = reader["LastName"].ToString() ?? "NULL";
                    string firstName = reader["FirstName"].ToString() ?? "NULL";
                    string patronymic = reader["Patronymic"].ToString() ?? "NULL";
                    string position = reader["Position"].ToString() ?? "NULL";
                    string login = reader["Login"].ToString() ?? "NULL";
                    string password = reader["Password"].ToString() ?? "NULL";

                    outputList.Add(new Employee(id, lastName, firstName, patronymic, position, login, password));
                }
            }
            return outputList;
        }
        public void InsertRow(Employee newEmployee)
        {
            string stringExpesstion = $"INSERT {this.table} VALUES (@LastName, @FirstName, @Patronymic, @Position, @Login, @Password)";
            SqlCommand command = new SqlCommand(stringExpesstion, this.connection);
            SqlParameter lastNameParam = new SqlParameter("@LastName", newEmployee.LastName);
            SqlParameter firstNameParam = new SqlParameter("@FirstName", newEmployee.FirstName);
            SqlParameter patronymicParam = new SqlParameter("@Patronymic", newEmployee.Patronymic);
            SqlParameter positionParam = new SqlParameter("@Position", newEmployee.Position);
            SqlParameter loginParam = new SqlParameter("@Login", newEmployee.Login);
            SqlParameter passwordParam = new SqlParameter("@Password", newEmployee.Password);
            command.Parameters.AddRange(new SqlParameter[] { firstNameParam, lastNameParam, patronymicParam, positionParam, loginParam, passwordParam });

            command.ExecuteNonQuery();
        }
        public void ChangeRow(int id,Employee changedEmployee)
        {
            string stringExpesstion = $"UPDATE {this.table} SET LastName=@LastName, FirstName=@FirstName, Patronymic=@Patronymic," +
                                      $"Position = @Position, Login = @Login, Password=@Password WHERE Id={id}";
            SqlCommand command = new SqlCommand(stringExpesstion, this.connection);
            SqlParameter lastNameParam = new SqlParameter("@LastName", changedEmployee.LastName);
            SqlParameter firstNameParam = new SqlParameter("@FirstName", changedEmployee.FirstName);
            SqlParameter patronymicParam = new SqlParameter("@Patronymic", changedEmployee.Patronymic);
            SqlParameter positionParam = new SqlParameter("@Position", changedEmployee.Position);
            SqlParameter loginParam = new SqlParameter("@Login", changedEmployee.Login);
            SqlParameter passwordParam = new SqlParameter("@Password", changedEmployee.Password);
            command.Parameters.AddRange(new SqlParameter[] { firstNameParam, lastNameParam, patronymicParam, positionParam, loginParam, passwordParam });

            command.ExecuteNonQuery();
        }
        public void DeleteRow(int id)
        {
            string stringExpesstion = $"DELETE FROM {this.table} WHERE Id={id}";
            SqlCommand command = new SqlCommand(stringExpesstion, this.connection);

            command.ExecuteNonQuery();
        }
    }
}
