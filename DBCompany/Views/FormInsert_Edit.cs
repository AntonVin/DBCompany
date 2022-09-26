using DBCompany;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBCompany.Views;

namespace DBCompany
{
    public partial class FormInsert_Edit : Form
    {
        private ModeForm modeForm;
        private int idEmployee;


        public event EventHandler ApplyEvent;

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
                txtboxPassword.Text = value.Password;
            }
        }

        public FormInsert_Edit(ModeForm modeForm)
        {
            InitializeComponent();
            this.modeForm = modeForm;
            SetForm();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            var txtBoxes = new TextBox[] { txtboxLastName, txtboxLogin, txtboxPassword };
            if (txtBoxes.Any(box=>string.IsNullOrWhiteSpace(box.Text)))
            {
                MessageBox.Show("Заполните обязательные поля");
                foreach (TextBox txtBox in txtBoxes)
                    txtbox_Validaiting(txtBox, null);
                return;
            }
            ApplyEvent?.Invoke(this,EventArgs.Empty);
            //this.DialogResult = DialogResult.OK;
        }

        private void txtbox_Validaiting(object sender, CancelEventArgs e)
        {
            if (sender is TextBox txtBox)
            {
                string error = String.IsNullOrEmpty(txtBox.Text) ? 
                               "Не указано значение" : String.Empty;
                errorProvider1.SetError(txtBox, error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtboxPassword.UseSystemPasswordChar = !viewPass.Checked;
        }


        private void SetForm()
        {
            switch (modeForm)
            {
                case ModeForm.Adding:
                    this.Text = "Добавление нового сотрудника";
                    this.btnApply.Text = "Добавить";
                    break;
                case ModeForm.Changing:
                    this.Text = "Изменение данных сотрудника";
                    this.btnApply.Text = "Изменить";
                    break;
                default:
                    break;
            }

            txtboxLastName.Validating += txtbox_Validaiting;
            txtboxLogin.Validating += txtbox_Validaiting;
            txtboxPassword.Validating += txtbox_Validaiting;
            txtboxPassword.UseSystemPasswordChar = true;
        }

        public void ShowMessageError(string message)
        {
 
                MessageBox.Show(message, "Некорректный ввод данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public enum ModeForm
        {
            Adding,
            Changing
        }
    }
}
