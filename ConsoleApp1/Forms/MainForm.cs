using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Model;
using ConsoleApp1.Views;

namespace ConsoleApp1.Forms
{
    public partial class MainForm : Form, IView
    {
        public event Action? ViewLoaded;
        public event Action? AddStudentRequested;
        public event Action? DeleteStudentRequested;

        public MainForm()
        {
            InitializeComponent();

            btnAdd.Click += (s, e) => AddStudentRequested?.Invoke();
            btnDelete.Click += (s, e) => DeleteStudentRequested?.Invoke();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ViewLoaded?.Invoke();
        }

        public string StudentName => txtName.Text;

        public string Speciality => txtSpeciality.Text;

        public string Group => txtGroup.Text;

        public int? SelectedStudentId
        {
            get
            {
                if (dgvStudents.CurrentRow != null && dgvStudents.CurrentRow.Tag is int id)
                {
                    return id;
                }
                return null;
            }
        }

        public void DisplayStudents(IReadOnlyList<Student> students)
        {
            dgvStudents.Rows.Clear();
            for (int i = 0; i < students.Count; i++)
            {
                var s = students[i];
                int rowIndex = dgvStudents.Rows.Add(i + 1, s.Name, s.Speciality, s.Group);
                dgvStudents.Rows[rowIndex].Tag = s.Id;
            }

            lblTotal.Text = $"Всего студентов: {students.Count}";
            btnDelete.Enabled = dgvStudents.Rows.Count > 0;
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void ClearInput()
        {
            txtName.Clear();
            txtSpeciality.Clear();
            txtGroup.Clear();
            txtName.Focus();
        }
    }
}
