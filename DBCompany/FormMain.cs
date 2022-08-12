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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var formSub = new FormInsert_Change(this,true);
            formSub.ShowDialog();
            if (formSub.IsConfirmed)
            {
                manipulator.InsertRow(formSub.Employee);
                FillDataGridView();
            }
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            if (dataGVEmployee.Rows.Count > 0)
            {
                DataGridViewRow dataGridViewRow = dataGVEmployee.CurrentRow;
                int id = Convert.ToInt32(dataGridViewRow.Cells[0].Value);
                string lastName = dataGridViewRow.Cells[1].Value.ToString();
                string firstName = dataGridViewRow.Cells[2].Value.ToString();
                string patronymic = dataGridViewRow.Cells[3].Value.ToString();
                string position = dataGridViewRow.Cells[4].Value.ToString();
                string login = dataGridViewRow.Cells[5].Value.ToString();
                string password = dataGridViewRow.Cells[6].Value.ToString();
                var formSub = new FormInsert_Change(this,false);
                formSub.Employee = new Employee(id, lastName, firstName, patronymic, position, login, password);
                formSub.ShowDialog();
                if(formSub.IsConfirmed)
                {
                    manipulator.ChangeRow(id, formSub.Employee);
                    FillDataGridView();
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
                //если надо передвигать колонки, то можно так
                //dataGVEmployee.Rows[rowNumber].Cells["Id"].Value = employees[i].Id;
                //dataGVEmployee.Rows[rowNumber].Cells["LastName"].Value = employees[i].LastName;
                //dataGVEmployee.Rows[rowNumber].Cells["FirstName"].Value = employees[i].FirstName;
                //dataGVEmployee.Rows[rowNumber].Cells["Patronymic"].Value = employees[i].Patronymic;
                //dataGVEmployee.Rows[rowNumber].Cells["Position"].Value = employees[i].Position;
                //dataGVEmployee.Rows[rowNumber].Cells["Login"].Value = employees[i].Login;
                //dataGVEmployee.Rows[rowNumber].Cells["Password"].Value = employees[i].Password;
            }
        }
    }
}