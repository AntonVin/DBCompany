using System.Collections;
using System.Data;
using DBCompany.Views;

namespace DBCompany
{
    public partial class FormMain : Form,IEmployeeView
    {
        public BindingSource Binding { get; set; } = new BindingSource();
        public Employee Employee
        {
            get
            {
                if (dataGVEmployee.Rows.Count > 0 && dataGVEmployee.CurrentRow == null)
                    dataGVEmployee.CurrentCell = dataGVEmployee.Rows[0].Cells[0];
                return new Employee(
                    Convert.ToInt32(dataGVEmployee.CurrentRow.Cells["Id"].Value),
                    dataGVEmployee.CurrentRow.Cells["LastName"].Value.ToString(),
                    dataGVEmployee.CurrentRow.Cells["FirstName"].Value.ToString(),
                    dataGVEmployee.CurrentRow.Cells["Patronymic"].Value.ToString(),
                    dataGVEmployee.CurrentRow.Cells["Position"].Value.ToString(),
                    dataGVEmployee.CurrentRow.Cells["Login"].Value.ToString(),
                    dataGVEmployee.CurrentRow.Cells["Password"].Value.ToString());
            }
         }

        public event EventHandler AddEvent;
        public event EventHandler EditEvent;
        public event EventHandler DeleteEvent;
        public event EventHandler UpdateEvent;
        
        public FormMain()
        {
            InitializeComponent();
            this.Load += (s, e) =>
                {
                    dataGVEmployee.DataSource = Binding.DataSource;
                    UpdateEvent?.Invoke(this, EventArgs.Empty);
                    SetDataGridView(dataGVEmployee);
                    EnablingButtons(this, EventArgs.Empty);
                };
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

            dataGVEmployee.RowsAdded += this.EnablingButtons;
            dataGVEmployee.RowsRemoved += this.EnablingButtons;

#if DEBUG
            dataGVEmployee.Columns["Id"].Visible = true;
#endif
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int indexRow =0;
            int indexColumn=0;
            if (dataGVEmployee.Rows.Count > 0)
            {
                dataGVEmployee.CurrentCell ??= dataGVEmployee.Rows[0].Cells[0];
                indexRow = dataGVEmployee.CurrentCell.RowIndex;
                indexColumn = dataGVEmployee.CurrentCell.ColumnIndex;
            }

            UpdateEvent?.Invoke(this,EventArgs.Empty);

            if (dataGVEmployee.Rows.Count > 0)
            {
                if (indexRow + 1 <= dataGVEmployee.Rows.Count)
                    dataGVEmployee.CurrentCell = dataGVEmployee.Rows[indexRow].Cells[indexColumn];
                else dataGVEmployee.CurrentCell = dataGVEmployee.Rows[^1].Cells[indexColumn];
            }
            EnablingButtons(this,EventArgs.Empty);
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEvent?.Invoke(this, EventArgs.Empty);
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            EditEvent?.Invoke(this, EventArgs.Empty);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить данного сотрудика?",
                                                    "УДАЛЕНИЕ", MessageBoxButtons.YesNo,MessageBoxIcon.Question,
                                                    MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes) 
                return;
            DeleteEvent?.Invoke(this, EventArgs.Empty);
        }

        private void dataGVEmployee_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnChange_Click(this,EventArgs.Empty);
        }


        private void EnablingButtons(object sender, EventArgs e)
        {
            bool enabled = dataGVEmployee.Rows.Count> 0;
            btnDelete.Enabled = enabled;
            btnChange.Enabled = enabled;
        }

        public void ShowMessageNotExistId()
        {
            var result = MessageBox.Show("Данного сотдруника удалил другой пользователь.\nОбновить таблицу?","ОШИБКА",
                MessageBoxButtons.YesNo,MessageBoxIcon.Error);
            if (result == DialogResult.Yes)
                UpdateEvent?.Invoke(this, EventArgs.Empty);
        }

    }
}