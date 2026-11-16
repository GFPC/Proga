using Model;

namespace ConsoleApp1.Views
{
    public interface IView
    {
        event Action ViewLoaded;
        event Action AddStudentRequested;
        event Action DeleteStudentRequested;

        string StudentName { get; }
        string Speciality { get; }
        string Group { get; }
        int? SelectedStudentId { get; }

        void DisplayStudents(IReadOnlyList<Student> students);
        void ShowError(string message);
        void ClearInput();
    }
}
