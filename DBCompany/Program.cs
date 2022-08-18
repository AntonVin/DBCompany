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
            try
            {
                var manipulator = new ManipulatorTableDB(connectionString, "Employees");
                ApplicationConfiguration.Initialize();
                Application.Run(new FormMain(manipulator));
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message,"Œÿ»¡ ¿",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}