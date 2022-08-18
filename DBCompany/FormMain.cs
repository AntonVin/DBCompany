namespace DBCompany
{
    public partial class FormMain : Form
    {
        ManipulatorTableDB manipulator;
        public List<string> Logins 
        {
            get{
                var ouput = new List<string>();
                foreach(DataGridViewRow row in dataGVEmployee.Rows)
                {
                    ouput.Add(row.Cells["Login"].Value.ToString());
                }
                return ouput;}
        }
        
        public FormMain(ManipulatorTableDB manipulator)
        {
            this.manipulator = manipulator;
            InitializeComponent();
            FillDataGridView();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int indexRow= dataGVEmployee.Rows.Count>0 ? dataGVEmployee.CurrentCell.RowIndex :0;
            int indexColumn= dataGVEmployee.Rows.Count > 0 ? dataGVEmployee.CurrentCell.ColumnIndex : 0;
            FillDataGridView();
            if (dataGVEmployee.Rows.Count > 0)
            {
                if (indexRow + 1 <= dataGVEmployee.Rows.Count)
                    dataGVEmployee.CurrentCell = dataGVEmployee.Rows[indexRow].Cells[indexColumn];
                else dataGVEmployee.CurrentCell = dataGVEmployee.Rows[^1].Cells[indexColumn];
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var formSub = new FormInsert_Change(this,true);
            if (formSub.ShowDialog() == DialogResult.OK)
            {
                manipulator.InsertRow(formSub.Employee);
                //dataGVEmployee.Rows.Add(
                //        formSub.Employee.Id, ---надо доработать Id
                //        formSub.Employee.LastName,
                //        formSub.Employee.FirstName,
                //        formSub.Employee.Patronymic,
                //        formSub.Employee.Position,
                //        formSub.Employee.Login,
                //        formSub.Employee.Password);
                FillDataGridView();
            }
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            if (dataGVEmployee.Rows.Count > 0)
            {
                DataGridViewRow dataGridViewRow = dataGVEmployee.CurrentRow;
                int id = Convert.ToInt32(dataGridViewRow.Cells[0].Value);
                Employee? changedEmployee = manipulator.GetEmployeeById(id);
                if (changedEmployee is null)
                {
                    DialogResult dialogResult = 
                        MessageBox.Show("Данного сотрудника не существует - таблица больше не актуальна.\nОбновить таблицу?","ОШИБКА",
                                        MessageBoxButtons.YesNo,MessageBoxIcon.Error);
                    if (dialogResult == DialogResult.Yes)
                        FillDataGridView();
                    return;
                }
                var formSub = new FormInsert_Change(this, modeAdding: false);
                formSub.Employee = changedEmployee;
                if(formSub.ShowDialog() == DialogResult.OK)
                {
                    manipulator.ChangeRow(formSub.Employee);
                    dataGridViewRow.SetValues(
                        formSub.Employee.Id,
                        formSub.Employee.LastName,
                        formSub.Employee.FirstName,
                        formSub.Employee.Patronymic,
                        formSub.Employee.Position,
                        formSub.Employee.Login,
                        formSub.Employee.Password);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGVEmployee.Rows.Count > 0)
            {
                int id = Convert.ToInt32(dataGVEmployee.CurrentRow.Cells["Id"].Value);
                manipulator.DeleteRow(id);
                FillDataGridView();
            }
        }

        private void FillDataGridView()
        {
            dataGVEmployee.Rows.Clear();
            List<Employee> employees = this.manipulator.GetDataList();
            for (int i = 0; i < employees.Count; i++)
            {
                int rowNumber = dataGVEmployee.Rows.Add(
                    employees[i].Id,
                    employees[i].LastName,
                    employees[i].FirstName,
                    employees[i].Patronymic,
                    employees[i].Position,
                    employees[i].Login,
                    employees[i].Password);
            }
        }


    }
}