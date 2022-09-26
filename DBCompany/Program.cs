using System.Data.SqlClient;
using DBCompany.Models;
using DBCompany.Views;
using DBCompany.Presenters;
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
            string connectionString = "Server=localhost;Database=Company;Trusted_Connection=True;Encrypt=False";
            try
            {
                ApplicationConfiguration.Initialize();
                var repository = new ModelEmployeeRepository(connectionString, "Employees");
                var view = new FormMain();
                var presenter = new EmployeePresenter(repository, view);
                Application.Run(view);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Œÿ»¡ ¿",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}