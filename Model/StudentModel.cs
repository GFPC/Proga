namespace Model
{
    public class StudentModel : IModel
    {
        private readonly List<Student> _students = new();
        private int _nextId = 1;

        public event Action? DataChanged;

        public StudentModel()
        {
            Add("Иванов Иван Иванович", "Информатика", "ПИ-101");
            Add("Петров Петр Петрович", "Информатика", "ПИ-101");
            Add("Сидорова Анна Сергеевна", "Математика", "М-201");
        }

        public IReadOnlyList<Student> GetAll()
        {
            return _students.AsReadOnly();
        }

        public void Add(string name, string speciality, string group)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("ФИО студента не может быть пустым.");
            if (string.IsNullOrWhiteSpace(speciality))
                throw new ArgumentException("Направление подготовки не может быть пустым.");
            if (string.IsNullOrWhiteSpace(group))
                throw new ArgumentException("Группа не может быть пустой.");

            _students.Add(new Student
            {
                Id = _nextId++,
                Name = name.Trim(),
                Speciality = speciality.Trim(),
                Group = group.Trim()
            });

            DataChanged?.Invoke();
        }

        public void Delete(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                _students.Remove(student);
                DataChanged?.Invoke();
            }
        }
    }
}
