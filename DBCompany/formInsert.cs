using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBCompany
{
    public partial class FormInsert_Change : Form
    {
        private bool modeAdding;
        private List<string> logins;
        public bool IsConfirmed { get; set; } = false;
        public Employee Employee
        {
            get 
            {
                return new Employee(null,
                    txtboxLastName.Text,
                    txtboxFirstName.Text,
                    txtboxPatronymic.Text,
                    txtboxPosition.Text,
                    txtboxLogin.Text,
                    txtboxPassword.Text
                );
            }
            set 
            {
                txtboxLastName.Text = value.LastName;
                txtboxFirstName.Text = value.FirstName;
                txtboxPatronymic.Text = value.Patronymic;
                txtboxPosition.Text = value.Position;
                txtboxLogin.Text = value.Login;
                txtboxPassword.Text = value.Password;
            }
        }
        public FormInsert_Change(FormMain form, bool modeAdding)
        {
            InitializeComponent();
            this.logins = form.Logins;
            this.modeAdding = modeAdding;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (this.logins.Contains(txtboxLogin.Text) && this.modeAdding==true)
            {
                MessageBox.Show("Такой логин уже существует.");
                txtboxLogin.Select();
                return;
            }
            if(string.IsNullOrWhiteSpace(txtboxLastName.Text) ||
                string.IsNullOrWhiteSpace(txtboxLogin.Text) || string.IsNullOrWhiteSpace(txtboxPassword.Text))
            {
                MessageBox.Show("Заполните обязательные поля");
                return;
            }
            IsConfirmed = true;
            this.Close();
        }
    }
}
