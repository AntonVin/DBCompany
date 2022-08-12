using System.Data.SqlClient;
namespace DBCompany
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string connectionString = "Server=localhost;Database=Company;Trusted_Connection=True;"; //MultipleActiveResultSets = true;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var manipulator = new ManipulatorTableDB(connection, "Employees");
                ApplicationConfiguration.Initialize();
                Application.Run(new FormMain(manipulator));
            }
        }
    }
}