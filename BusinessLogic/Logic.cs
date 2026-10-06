using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataAccessLayer;
using DataAccessLayer.EF;
using Model;

namespace BusinessLogic
{
    public class Logic
    {
        public IRepository<Student> repository { get; set; }

        public Logic() : this(new EntityRepository<Student>())
        {
        }

        public Logic(IRepository<Student> repository)
        {
            this.repository = repository;
        }

        public void AddStudent(string name, string speciality, string group)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("ФИО студента не может быть пустым.", nameof(name));
            if (string.IsNullOrWhiteSpace(speciality))
                throw new ArgumentException("Направление подготовки (специальность) не может быть пустым.", nameof(speciality));
            if (string.IsNullOrWhiteSpace(group))
                throw new ArgumentException("Группа не может быть пустой.", nameof(group));

            repository.Add(new Student
            {
                Name = name.Trim(),
                Speciality = speciality.Trim(),
                Group = group.Trim()
            });
        }

        public void DeleteStudent(string name, string speciality, string group)
        {
            var students = repository.ReadAll();
            var student = students.FirstOrDefault(s =>
                string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(s.Speciality, speciality, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(s.Group, group, StringComparison.OrdinalIgnoreCase));

            if (student != null)
            {
                repository.Delete(student.Id);
            }
        }

        public bool DeleteStudentAt(int index)
        {
            var students = repository.ReadAll();
            if (index >= 0 && index < students.Count)
            {
                var student = students[index];
                repository.Delete(student.Id);
                return true;
            }
            return false;
        }

        public IReadOnlyList<StudentRecord> GetStudentsRecords()
        {
            var students = repository.ReadAll();
            var records = new List<StudentRecord>(students.Count);
            for (int i = 0; i < students.Count; i++)
            {
                var s = students[i];
                records.Add(new StudentRecord(i + 1, s.Name, s.Speciality, s.Group));
            }
            return records;
        }

        public DataTable GetStudentsDataTable()
        {
            var students = repository.ReadAll();
            var table = new DataTable("Students");
            table.Columns.Add("№", typeof(int));
            table.Columns.Add("ФИО студента", typeof(string));
            table.Columns.Add("Направление подготовки", typeof(string));
            table.Columns.Add("Группа", typeof(string));

            for (int i = 0; i < students.Count; i++)
            {
                var s = students[i];
                table.Rows.Add(i + 1, s.Name, s.Speciality, s.Group);
            }

            return table;
        }

        public Dictionary<string, int> GetSpecialityDistribution()
        {
            var students = repository.ReadAll();
            return students
                .GroupBy(s => s.Speciality.Trim(), StringComparer.OrdinalIgnoreCase)
                .OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public void SeedInitialData()
        {
            var currentStudents = repository.ReadAll();
            if (currentStudents.Count == 0)
            {
                AddStudent("Иванов Иван Иванович", "Программная инженерия", "ПИ-21");
                AddStudent("Петров Петр Сергеевич", "Информатика и вычислительная техника", "ИВТ-22");
                AddStudent("Сидорова Анна Алексеевна", "Программная инженерия", "ПИ-21");
                AddStudent("Кузнецов Дмитрий Олегович", "Информационная безопасность", "ИБ-21");
                AddStudent("Смирнова Мария Павловна", "Программная инженерия", "ПИ-22");
                AddStudent("Васильев Алексей Игоревич", "Информатика и вычислительная техника", "ИВТ-21");
                AddStudent("Морозова Елена Викторовна", "Прикладная математика", "ПМ-21");
            }
        }
    }
}
