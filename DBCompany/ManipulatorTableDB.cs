using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DBCompany
{
    public class ManipulatorTableDB
    {
        DataTable dataTable;
        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;

        public string ConnectionString { get; set; }
        public string Table { get; set; }
        public DataTable DataTable
        {
            get { return dataTable; }
        }

        public ManipulatorTableDB(string connectionString, string table)
        {
            this.ConnectionString = connectionString;
            this.Table = table;
            dataTable = new DataTable();
            adapter = new SqlDataAdapter($"SELECT * FROM {this.Table}", connectionString);
            commandBuilder = new SqlCommandBuilder(adapter);
        }

        public void RefreshTable()
        {
            dataTable.Clear();
            adapter.Fill(dataTable);
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

        public bool InsertRow(Employee newEmployee)
        {
            string query = $"INSERT INTO Employees (LastName,FirstName,Patronymic,Position,Login,Password) VALUES"+
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


            DataRow newRow = dataTable.NewRow();
            newRow.ItemArray = new object[]{
                -1,
                newEmployee.LastName,
                newEmployee.FirstName,
                newEmployee.Patronymic,
                newEmployee.Position,
                newEmployee.Login,
                newEmployee.Password,
                };
            dataTable.Rows.Add(newRow);
            adapter.Update(dataTable);
            return true;
        }

        public bool ChangeRow(Employee changedEmployee)
        {
            DataRow row = dataTable.Select($"Id={changedEmployee.Id}").First();
            row.ItemArray = new object[]{
                changedEmployee.Id,
                changedEmployee.LastName,
                changedEmployee.FirstName,
                changedEmployee.Patronymic,
                changedEmployee.Position,
                changedEmployee.Login,
                changedEmployee.Password,
                };
            adapter.Update(dataTable);
            return true;
        }

        public bool DeleteRow(int id)
        {
            DataRow row = dataTable.Select($"Id={id}").First();
            row.Delete();
            adapter.Update(dataTable);
            return true;
        }

    }
}
