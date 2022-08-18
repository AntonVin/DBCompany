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
        private int idEmployee;
        private string currentLogin;
        private List<string> logins;
        public Employee Employee
        {
            get 
            {
                return new Employee(
                    idEmployee,
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
                idEmployee = value.Id;
                txtboxLastName.Text = value.LastName;
                txtboxFirstName.Text = value.FirstName;
                txtboxPatronymic.Text = value.Patronymic;
                txtboxPosition.Text = value.Position;
                txtboxLogin.Text = value.Login;
                currentLogin = value.Login;
                txtboxPassword.Text = value.Password;
            }
        }
        public FormInsert_Change(FormMain form, bool modeAdding)
        {
            InitializeComponent();
            this.logins = form.Logins;
            this.modeAdding = modeAdding;
            txtboxLastName.Validating += txtbox_Validaiting;
            txtboxLogin.Validating += txtbox_Validaiting;
            txtboxPassword.Validating += txtbox_Validaiting;
            txtboxPassword.UseSystemPasswordChar = true;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            string newLogin = txtboxLogin.Text;
            if (this.modeAdding == true && this.logins.Contains(txtboxLogin.Text)
                || (this.modeAdding == false && (this.logins.Contains(txtboxLogin.Text) && newLogin!=currentLogin)))
            {
                MessageBox.Show("Такой логин уже существует.");
                txtboxLogin.Select();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtboxLastName.Text) ||
                string.IsNullOrWhiteSpace(txtboxLogin.Text) || string.IsNullOrWhiteSpace(txtboxPassword.Text))
            {
                MessageBox.Show("Заполните обязательные поля");
                txtbox_Validaiting(txtboxLastName, null);
                txtbox_Validaiting(txtboxLogin, null);
                txtbox_Validaiting(txtboxPassword, null);
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void txtbox_Validaiting(object sender, CancelEventArgs e)
        {
            if(sender is TextBox txtBox)
            if (String.IsNullOrEmpty(txtBox.Text))
            {
                errorProvider1.SetError(txtBox, "Не указано значение");
            }
            else
            {
                errorProvider1.SetError(txtBox, String.Empty);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkPass.Checked) txtboxPassword.UseSystemPasswordChar = false;
            else txtboxPassword.UseSystemPasswordChar = true;
        }
    }
}
