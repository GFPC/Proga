using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic;

namespace ConsoleApp1.Cli
{
    public static class CliView
    {
        public static void Run(Logic logic)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine();
            Console.WriteLine("===============================================================");
            Console.WriteLine("                DecanatPRO — Консольный режим (CLI)");
            Console.WriteLine("===============================================================");

            bool running = true;
            while (running)
            {
                PrintMenu();
                Console.Write("Выберите действие (1-5): ");
                string choice = ReadInput();
                if (string.IsNullOrEmpty(choice) && Console.IsInputRedirected)
                {
                    // If stream ended
                    running = false;
                    break;
                }
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ShowStudentsTable(logic);
                        break;
                    case "2":
                        AddNewStudent(logic);
                        break;
                    case "3":
                        DeleteStudent(logic);
                        break;
                    case "4":
                        ShowHistogram(logic);
                        break;
                    case "5":
                        running = false;
                        Console.WriteLine("Выход из программы. До свидания!");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Некорректный выбор. Пожалуйста, введите цифру от 1 до 5.");
                        Console.ResetColor();
                        break;
                }

                if (running)
                {
                    Console.WriteLine();
                    Console.Write("Нажмите Enter для продолжения...");
                    Console.ReadLine();
                    Console.WriteLine();
                }
            }
        }

        private static void PrintMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------- ГЛАВНОЕ МЕНЮ -------------------------");
            Console.ResetColor();
            Console.WriteLine(" 1. Вывести список студентов (таблица)");
            Console.WriteLine(" 2. Добавить нового студента");
            Console.WriteLine(" 3. Удалить студента");
            Console.WriteLine(" 4. Показать гистограмму распределения по специальностям");
            Console.WriteLine(" 5. Завершить работу");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("----------------------------------------------------------------");
            Console.ResetColor();
        }

        private static void ShowStudentsTable(Logic logic)
        {
            var records = logic.GetStudentsRecords();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("======================= СПИСОК СТУДЕНТОВ =======================");
            Console.ResetColor();

            if (records.Count == 0)
            {
                Console.WriteLine("Список студентов пуст.");
                return;
            }

            int colNumWidth = 4;
            int colNameWidth = Math.Max(25, records.Max(r => r.Name.Length) + 2);
            int colSpecWidth = Math.Max(35, records.Max(r => r.Speciality.Length) + 2);
            int colGroupWidth = Math.Max(10, records.Max(r => r.Group.Length) + 2);

            string line = "+" + new string('-', colNumWidth) + "+"
                          + new string('-', colNameWidth) + "+"
                          + new string('-', colSpecWidth) + "+"
                          + new string('-', colGroupWidth) + "+";

            Console.WriteLine(line);
            Console.WriteLine($"|{" №".PadRight(colNumWidth)}|{" ФИО".PadRight(colNameWidth)}|{" Специальность".PadRight(colSpecWidth)}|{" Группа".PadRight(colGroupWidth)}|");
            Console.WriteLine(line);

            foreach (var r in records)
            {
                Console.WriteLine($"| {r.Index.ToString().PadRight(colNumWidth - 1)}| {r.Name.PadRight(colNameWidth - 1)}| {r.Speciality.PadRight(colSpecWidth - 1)}| {r.Group.PadRight(colGroupWidth - 1)}|");
            }

            Console.WriteLine(line);
            Console.WriteLine($"Всего студентов: {records.Count}");
        }

        private static void AddNewStudent(Logic logic)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==================== ДОБАВЛЕНИЕ СТУДЕНТА ====================");
            Console.ResetColor();

            Console.Write("Введите ФИО студента: ");
            string name = ReadInput();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: ФИО не может быть пустым.");
                Console.ResetColor();
                return;
            }

            Console.Write("Введите специальность (направление подготовки): ");
            string speciality = ReadInput();
            if (string.IsNullOrWhiteSpace(speciality))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: специальность не может быть пустой.");
                Console.ResetColor();
                return;
            }

            Console.Write("Введите номер группы: ");
            string group = ReadInput();
            if (string.IsNullOrWhiteSpace(group))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: группа не может быть пустой.");
                Console.ResetColor();
                return;
            }

            try
            {
                logic.AddStudent(name, speciality, group);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[Успешно] Студент '{name}' добавлен!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка при добавлении: {ex.Message}");
                Console.ResetColor();
            }
        }

        private static void DeleteStudent(Logic logic)
        {
            var records = logic.GetStudentsRecords();
            if (records.Count == 0)
            {
                Console.WriteLine("Список студентов пуст. Некого удалять.");
                return;
            }

            ShowStudentsTable(logic);
            Console.WriteLine();
            Console.Write("Введите номер студента (№) для удаления (или 0 для отмены): ");

            if (!int.TryParse(ReadInput(), out int num) || num < 0 || num > records.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Некорректный номер.");
                Console.ResetColor();
                return;
            }

            if (num == 0)
            {
                Console.WriteLine("Удаление отменено.");
                return;
            }

            var target = records[num - 1];
            Console.Write($"Вы действительно хотите удалить студента '{target.Name}' ({target.Group})? (y/n): ");
            string confirm = ReadInput().ToLowerInvariant();

            if (confirm == "y" || confirm == "yes" || confirm == "д" || confirm == "да")
            {
                bool deleted = logic.DeleteStudentAt(num - 1);
                if (deleted)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[Успешно] Студент удален!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ошибка при удалении.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("Удаление отменено пользователем.");
            }
        }

        private static string ReadInput()
        {
            string? s = Console.ReadLine();
            return s == null ? string.Empty : s.Trim().Trim('\uFEFF');
        }

        private static void ShowHistogram(Logic logic)
        {
            var distribution = logic.GetSpecialityDistribution();
            int total = distribution.Values.Sum();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("================== ГИСТОГРАММА СПЕЦИАЛЬНОСТЕЙ ==================");
            Console.ResetColor();

            if (distribution.Count == 0 || total == 0)
            {
                Console.WriteLine("Нет данных о студентах для построения гистограммы.");
                return;
            }

            int maxLabelWidth = Math.Max(20, distribution.Keys.Max(k => k.Length));
            int maxCount = distribution.Values.Max();
            const int maxBarWidth = 30;

            foreach (var kvp in distribution)
            {
                int count = kvp.Value;
                double percentage = (double)count / total * 100.0;
                int barLen = (int)Math.Round((double)count / maxCount * maxBarWidth);
                if (barLen < 1 && count > 0) barLen = 1;

                string bar = new string('█', barLen);
                string label = kvp.Key.PadRight(maxLabelWidth);

                Console.Write($"{label} | [");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{count,2}");
                Console.ResetColor();
                Console.Write($"] ({percentage,5:F1}%) ");

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(bar);
                Console.ResetColor();
            }

            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine($"Всего специальностей: {distribution.Count}, Всего студентов: {total}");
            Console.WriteLine("================================================================");
        }
    }
}
