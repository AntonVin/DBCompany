using System.Data;

namespace DBCompany
{
    public partial class FormMain : Form
    {
        ManipulatorTableDB manipulator;
        DataController controller;
        
        public FormMain(ManipulatorTableDB manipulator)
        {
            this.manipulator = manipulator;
            this.controller = new DataController(manipulator);
            InitializeComponent();
            dataGVEmployee.DataSource = manipulator.DataTable;
            manipulator.RefreshTable();
            manipulator.DataTable.RowDeleting += new DataRowChangeEventHandler(this.EnablingButtons);
            manipulator.DataTable.TableNewRow += new DataTableNewRowEventHandler(this.EnablingButtons);
            SetDataGridView(dataGVEmployee);
#if DEBUG
            dataGVEmployee.Columns["Id"].Visible = true;
#endif
        }

        private void SetDataGridView(DataGridView dgv)
        {
            dgv.Columns["Id"].Visible = false;
            dgv.Columns["Password"].Visible = false;
            string[] namesColumns = { "Id", "Фамилия", "Имя", "Отчество", "Должность", "Логин", "Пароль" };
            for(int i=0;i< dgv.Columns.Count;i++)
                dgv.Columns[i].HeaderText = namesColumns[i];
            //вырви глаз
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font(dgv.ColumnHeadersDefaultCellStyle.Font.FontFamily, 10f, FontStyle.Bold | FontStyle.Italic);
            dgv.BackgroundColor = Color.LightSlateGray;
            dgv.BorderStyle = BorderStyle.Fixed3D;
            dgv.GridColor = Color.Yellow;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.BlueViolet;
            dgv.ForeColor = Color.Red;
            dgv.DefaultCellStyle.BackColor = Color.GreenYellow;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Red;
            //dgv.ColumnHeadersDefaultCellStyle.Font = Fo;

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int indexRow= dataGVEmployee.Rows.Count>0 ? dataGVEmployee.CurrentCell.RowIndex :0;
            int indexColumn= dataGVEmployee.Rows.Count > 0 ? dataGVEmployee.CurrentCell.ColumnIndex : 0;
            manipulator.RefreshTable();
            if (dataGVEmployee.Rows.Count > 0)
            {
                if (indexRow + 1 <= dataGVEmployee.Rows.Count)
                    dataGVEmployee.CurrentCell = dataGVEmployee.Rows[indexRow].Cells[indexColumn];
                else dataGVEmployee.CurrentCell = dataGVEmployee.Rows[^1].Cells[indexColumn];
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var formSub = new FormInsert_Change(this, FormInsert_Change.ModeForm.Adding, manipulator);
            formSub.ShowDialog();
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            if (dataGVEmployee.Rows.Count > 0)
            {
                DataGridViewRow dataGridViewRow = dataGVEmployee.CurrentRow;
                int id = Convert.ToInt32(dataGridViewRow.Cells[0].Value);
                if (!controller.ExistId(id))
                {
                    DialogResult dialogResult = 
                        MessageBox.Show($"{controller.TextError}\nОбновить список сотрудников?","ОШИБКА",
                                        MessageBoxButtons.YesNo,MessageBoxIcon.Error);
                    if (dialogResult == DialogResult.Yes)
                        manipulator.RefreshTable();
                    return;
                }
                var formSub = new FormInsert_Change(this, FormInsert_Change.ModeForm.Changing,manipulator);
                formSub.Employee = new Employee(id,
                    dataGridViewRow.Cells["LastName"].Value.ToString(),
                    dataGridViewRow.Cells["FirstName"].Value.ToString(),
                    dataGridViewRow.Cells["Patronymic"].Value.ToString(),
                    dataGridViewRow.Cells["Position"].Value.ToString(),
                    dataGridViewRow.Cells["Login"].Value.ToString(),
                    dataGridViewRow.Cells["Password"].Value.ToString());
                formSub.ShowDialog();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGVEmployee.Rows.Count <= 0) return;
            DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить данного сотрудика?",
                                "УДАЛЕНИЕ", MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
            if (dialogResult != DialogResult.Yes) return;

            int id = Convert.ToInt32(dataGVEmployee.CurrentRow.Cells["Id"].Value);
            if (!controller.Delete(id))
            {
                dialogResult = MessageBox.Show($"{controller.TextError}\nОбновить список сотрудников?", 
                                    "ОШИБКА", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dialogResult == DialogResult.Yes)
                    manipulator.RefreshTable();
            }
        }

        private void dataGVEmployee_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnChange_Click(null,null);
        }


        private void EnablingButtons(object sender, EventArgs e)
        {
            bool enabled = dataGVEmployee.Rows.Count> 0;
            btnDelete.Enabled = enabled;
            btnChange.Enabled = enabled;
        }
        private void dataGVEmployee_Validated(object sender, EventArgs e)
        {

        }
    }
}