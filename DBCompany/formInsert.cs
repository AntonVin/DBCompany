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
        private DataController controller;
        private ModeForm modeForm;
        private int idEmployee;
        private string currentLogin;
        private ManipulatorTableDB manipulator;

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

        public FormInsert_Change(FormMain form, ModeForm modeForm, ManipulatorTableDB manipulator)
        {
            InitializeComponent();
            this.modeForm = modeForm;
            this.manipulator = manipulator;
            this.controller = new DataController(manipulator);
            SetForm();
        }

        private void SetForm()
        {
            txtboxLastName.Validating += txtbox_Validaiting;
            txtboxLogin.Validating += txtbox_Validaiting;
            txtboxPassword.Validating += txtbox_Validaiting;
            txtboxPassword.UseSystemPasswordChar = true;
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

            bool successedOperation = modeForm == ModeForm.Adding ?
                                      controller.Insert(this.Employee):
                                      controller.Change(this.Employee);
            if (!successedOperation)
            {
                MessageBox.Show(controller.TextError,"Некорректный ввод данных",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
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

        public enum ModeForm
        {
            Adding,
            Changing
        }
    }
}
