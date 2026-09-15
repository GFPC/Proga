using System;
using System.Drawing;
using System.Windows.Forms;
using BusinessLogic;

namespace ConsoleApp1.Forms
{
    public partial class MainForm : Form
    {
        private readonly Logic _logic;

        public MainForm(Logic logic)
        {
            _logic = logic;
            InitializeComponent();
        }

        public MainForm() : this(new Logic())
        {
            _logic.SeedInitialData();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RefreshData();
        }

        private void RefreshData()
        {
            dgvStudents.DataSource = _logic.GetStudentsDataTable();

            // Style columns
            if (dgvStudents.Columns.Count > 0)
            {
                if (dgvStudents.Columns["№"] is DataGridViewColumn colNum)
                {
                    colNum.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    colNum.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvStudents.Columns["ФИО студента"] is DataGridViewColumn colName)
                {
                    colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    colName.FillWeight = 40;
                }
                if (dgvStudents.Columns["Направление подготовки"] is DataGridViewColumn colSpec)
                {
                    colSpec.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    colSpec.FillWeight = 40;
                }
                if (dgvStudents.Columns["Группа"] is DataGridViewColumn colGroup)
                {
                    colGroup.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    colGroup.FillWeight = 20;
                }
            }

            lblTotal.Text = $"Всего студентов: {_logic.GetStudentsRecords().Count}";
            btnDelete.Enabled = dgvStudents.Rows.Count > 0;
            pnlHistogram.Invalidate();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using var form = new AddStudentForm();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    _logic.AddStudent(form.StudentName, form.StudentSpeciality, form.StudentGroup);
                    RefreshData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null || dgvStudents.CurrentRow.Index < 0)
            {
                MessageBox.Show("Пожалуйста, выберите студента в таблице для удаления.", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int selectedIndex = dgvStudents.CurrentRow.Index;
            string studentName = dgvStudents.CurrentRow.Cells["ФИО студента"].Value?.ToString() ?? "выбранного студента";

            var result = MessageBox.Show(
                $"Вы действительно хотите удалить студента:\n\"{studentName}\"?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool deleted = _logic.DeleteStudentAt(selectedIndex);
                if (deleted)
                {
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Не удалось удалить студента.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnOpenHistogram_Click(object sender, EventArgs e)
        {
            using var form = new HistogramForm(_logic);
            form.ShowDialog(this);
        }

        private void pnlHistogram_Paint(object sender, PaintEventArgs e)
        {
            HistogramForm.RenderHistogram(e.Graphics, pnlHistogram.ClientRectangle, _logic);
        }

        private void pnlHistogram_Resize(object sender, EventArgs e)
        {
            pnlHistogram.Invalidate();
        }
    }
}
