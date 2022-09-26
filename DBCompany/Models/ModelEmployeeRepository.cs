using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using DBCompany.Models;

namespace DBCompany
{
    public class ModelEmployeeRepository : IEmployeeRepository
    {
        //Fields
        string tableName;
        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;

        //Properties
        public string ConnectionString { get; set; }
        public DataTable Table { get; set; }

        //Constructor
        public ModelEmployeeRepository(string connectionString, string table)
        {
            this.ConnectionString = connectionString;
            this.tableName = table;
            this.Table = new DataTable();
            adapter = new SqlDataAdapter($"SELECT * FROM {this.tableName}", connectionString);
            commandBuilder = new SqlCommandBuilder(adapter);
        }

        //Methods
        void IEmployeeRepository.UpdateRepository()
        {
            this.Table.Clear();
            this.adapter.Fill(Table);
        }

        void IEmployeeRepository.Add(Employee newEmployee)
        {
            string query = $"INSERT INTO {this.tableName} (LastName,FirstName,Patronymic,Position,Login,Password) VALUES"+
                $"('{newEmployee.LastName}','{newEmployee.FirstName}','{newEmployee.Patronymic}','{newEmployee.Position}',"+
                $"'{newEmployee.Login}','{newEmployee.Password}')" + ";SET @Id = SCOPE_IDENTITY()";

            SqlParameter parameterId = new SqlParameter("@Id", SqlDbType.Int, 0, "Id");
            parameterId.Direction = ParameterDirection.Output;
            SqlParameter[] parameters =
            {
                parameterId,
                new SqlParameter("@LastName", newEmployee.LastName),
                new SqlParameter("@FirstName", newEmployee.FirstName),
                new SqlParameter("@Patronymic", newEmployee.Patronymic),
                new SqlParameter("@Position", newEmployee.Position),
                new SqlParameter("@Login", newEmployee.Login),
                new SqlParameter("@Password", newEmployee.Password)
            };

            adapter.InsertCommand = new SqlCommand(query);
            adapter.InsertCommand.Parameters.AddRange(parameters);


            DataRow newRow = this.Table.NewRow();
            newRow.ItemArray = new object[]{
                -1,
                newEmployee.LastName,
                newEmployee.FirstName,
                newEmployee.Patronymic,
                newEmployee.Position,
                newEmployee.Login,
                newEmployee.Password,
                };
            this.Table.Rows.Add(newRow);
            this.adapter.Update(this.Table);
        }

        void IEmployeeRepository.Edit(Employee changedEmployee)
        {
            DataRow row = this.Table.Select($"Id={changedEmployee.Id}").First();
            row.ItemArray = new object[]{
                changedEmployee.Id,
                changedEmployee.LastName,
                changedEmployee.FirstName,
                changedEmployee.Patronymic,
                changedEmployee.Position,
                changedEmployee.Login,
                changedEmployee.Password,
                };
            this.adapter.Update(this.Table);
        }

        void IEmployeeRepository.Delete(int id)
        {
            DataRow row = this.Table.Select($"Id={id}").First();
            row.Delete();
            this.adapter.Update(this.Table);
        }

        bool IEmployeeRepository.CheckUniqueFullNameByOtherId(Employee employee)
        {

            string query = $"SELECT * FROM {this.tableName} " +
                $"WHERE Id<>{employee.Id} AND LastName='{employee.LastName}' AND FirstName='{employee.FirstName}' AND " +
                $"Patronymic ='{employee.Patronymic}' AND Position ='{employee.Position}'";
            using var connection = new SqlConnection(this.ConnectionString);
            connection.Open();
            var command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();
            bool unique = !reader.HasRows;
            reader.Close();
            return unique;
        }
        bool IEmployeeRepository.CheckUniqueLoginByOtherId(Employee employee)
        {
            string query = $"SELECT * FROM {this.tableName} " +
                $"WHERE Id<>{employee.Id} AND Login='{employee.Login}'";
            using var connection = new SqlConnection(this.ConnectionString);
            connection.Open();
            var command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();
            bool unique = !reader.HasRows;
            reader.Close();
            return unique;
        }

        bool IEmployeeRepository.ExistId(int id)
        {
            using var connection = new SqlConnection(this.ConnectionString);
            connection.Open();
            string query = $"SELECT * FROM {this.tableName} WHERE Id={id}";
            var command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();
            bool exist = reader.HasRows;
            reader.Close();
            return exist;
        }

        //public Employee? GetEmployeeById(int id)
        //{
        //    using var connection = new SqlConnection(this.ConnectionString);
        //    connection.Open();

        //    string query = $"SELECT * FROM {this.Table} WHERE Id={id}";
        //    var command = new SqlCommand(query, connection);
        //    using (SqlDataReader reader = command.ExecuteReader())
        //    {
        //        if (!reader.Read()) return null;
        //        string lastName = reader["LastName"].ToString();
        //        string firstName = reader["FirstName"].ToString();
        //        string patronymic = reader["Patronymic"].ToString();
        //        string position = reader["Position"].ToString();
        //        string login = reader["Login"].ToString();
        //        string password = reader["Password"].ToString();
        //        return new Employee(id, lastName, firstName, patronymic, position, login, password);
        //    }
        //}
    }
}
