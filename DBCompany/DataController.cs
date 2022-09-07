using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DBCompany
{
    internal class DataController
    {
        ManipulatorTableDB manipulator;
        public string TextError { get; private set; } = "Неизвестная ошибка";
        public DataController(ManipulatorTableDB manipulator)
        {
            this.manipulator = manipulator;
        }

        public bool Delete(int id)
        {
            if (!ExistId(id)) 
                return false;
            manipulator.DeleteRow(id);
            return true;
        }
        public bool Insert(Employee newEmployee)
        {
            if(!CheckUniqueFullNameByOtherId(newEmployee))
                return false;
            if(!CheckUniqueLoginByOtherId(newEmployee))
                return false;
            manipulator.InsertRow(newEmployee);
            return true;
        }
        public bool Change(Employee changedEmployee)
        {
            if(!ExistId(changedEmployee.Id))
                return false;
            if (!CheckUniqueFullNameByOtherId(changedEmployee))
                return false;
            if (!CheckUniqueLoginByOtherId(changedEmployee))
                return false;
            manipulator.ChangeRow(changedEmployee);
            return true;
        }
        public bool ExistId(int id)
        {
            using var connection = new SqlConnection(manipulator.ConnectionString);
            connection.Open();
            string query = $"SELECT * FROM {manipulator.Table} WHERE Id={id}";
            var command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();
            bool exist = reader.HasRows;
            reader.Close();
            if (!exist)
                this.TextError = "Этот сотрудник уже удалён другим пользователем";
            return exist;
        }
        private bool CheckUniqueFullNameByOtherId(Employee employee)
        {

            string query = $"SELECT * FROM {manipulator.Table} " +
                $"WHERE Id<>{employee.Id} AND LastName='{employee.LastName}' AND FirstName='{employee.FirstName}' AND " +
                $"Patronymic ='{employee.Patronymic}' AND Position ='{employee.Position}'";
            using var connection = new SqlConnection(manipulator.ConnectionString);
            connection.Open();
            var command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();
            bool unique = !reader.HasRows;
            reader.Close();
            if (!unique) 
                this.TextError = "Сотрудник с таким ФИО и должностью уже существует";
            return unique;
        }
        private bool CheckUniqueLoginByOtherId(Employee employee)
        {
            string query = $"SELECT * FROM {manipulator.Table} " +
                $"WHERE Id<>{employee.Id} AND Login='{employee.Login}'";
            using var connection = new SqlConnection(manipulator.ConnectionString);
            connection.Open();
            var command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();
            bool unique = !reader.HasRows;
            reader.Close();
            if(!unique)
                this.TextError = "Сотрудник с таким логином уже существует";
            return unique;
        }
    }
}
