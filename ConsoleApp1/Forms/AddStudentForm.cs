using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConsoleApp1.Forms
{
    public partial class AddStudentForm : Form
    {
        public string StudentName => txtName.Text.Trim();
        public string StudentSpeciality => cmbSpeciality.Text.Trim();
        public string StudentGroup => txtGroup.Text.Trim();

        public AddStudentForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите ФИО студента.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(cmbSpeciality.Text))
            {
                MessageBox.Show("Введите или выберите специальность (направление подготовки).", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSpeciality.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtGroup.Text))
            {
                MessageBox.Show("Введите номер группы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGroup.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
