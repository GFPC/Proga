using System;
using System.Windows.Forms;
using Model;
using ConsoleApp1.Forms;
using ConsoleApp1.Views;

namespace ConsoleApp1.Presenters
{
    public class Presenter
    {
        private readonly IModel _model;
        private readonly IView _view;

        public Presenter()
        {
            _model = new StudentModel();
            _view = new MainForm();

            _view.ViewLoaded += OnViewLoaded;
            _view.AddStudentRequested += OnAddStudentRequested;
            _view.DeleteStudentRequested += OnDeleteStudentRequested;

            _model.DataChanged += OnModelDataChanged;
        }

        public void Run()
        {
            Application.Run((Form)_view);
        }

        private void OnViewLoaded()
        {
            _view.DisplayStudents(_model.GetAll());
        }

        private void OnModelDataChanged()
        {
            _view.DisplayStudents(_model.GetAll());
        }

        private void OnAddStudentRequested()
        {
            try
            {
                _model.Add(_view.StudentName, _view.Speciality, _view.Group);
                _view.ClearInput();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OnDeleteStudentRequested()
        {
            var id = _view.SelectedStudentId;
            if (!id.HasValue)
            {
                _view.ShowError("Выберите студента из списка для удаления.");
                return;
            }

            _model.Delete(id.Value);
        }
    }
}
