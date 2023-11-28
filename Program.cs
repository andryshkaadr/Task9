namespace Task9
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        static ScoreManager scoreManager;

        static Program()
        {
            AppDomain.CurrentDomain.ProcessExit += (sender, e) => SaveDataOnExit();
            scoreManager = new ScoreManager();
            scoreManager.LoadData();
        }

        static void SaveDataOnExit()
        {
            scoreManager.SaveData();
            Console.WriteLine("Данные сохранены при выходе (Binary и JSON).");
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Удалить студента");
                Console.WriteLine("3. Добавить оценку");
                Console.WriteLine("4. Удалить оценку");
                Console.WriteLine("5. Получить оценку студента");
                Console.WriteLine("6. Получить всех студентов с оценками");
                Console.WriteLine("7. Сортировать студентов");
                Console.WriteLine("8. Фильтровать студентов");
                Console.WriteLine("9. Выйти");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.Write("Введите имя студента: ");
                            string studentName = Console.ReadLine();
                            scoreManager.AddStudent(studentName, new Dictionary<string, int>());
                            break;

                        case 2:
                            Console.Write("Введите имя студента: ");
                            studentName = Console.ReadLine();
                            scoreManager.RemoveStudent(studentName);
                            break;

                        case 3:
                            Console.Write("Введите имя студента: ");
                            studentName = Console.ReadLine();
                            Console.Write("Введите предмет: ");
                            string subject = Console.ReadLine();
                            if (int.TryParse(Console.ReadLine(), out int score))
                            {
                                scoreManager.AddScore(studentName, subject, score);
                            }
                            else
                            {
                                Console.WriteLine("Неверная оценка. Введите корректное число.");
                            }
                            break;

                        case 4:
                            Console.Write("Введите имя студента: ");
                            studentName = Console.ReadLine();
                            Console.Write("Введите предмет: ");
                            subject = Console.ReadLine();
                            scoreManager.RemoveScore(studentName, subject);
                            break;

                        case 5:
                            Console.Write("Введите имя студента: ");
                            studentName = Console.ReadLine();
                            Console.Write("Введите предмет: ");
                            subject = Console.ReadLine();
                            int studentScore = scoreManager.GetStudentScore(studentName, subject);
                            if (studentScore != -1)
                            {
                                Console.WriteLine($"{studentName} получил по предмету {subject}: {studentScore}");
                            }
                            else
                            {
                                Console.WriteLine("Студент не найден или нет оценки по предмету.");
                            }
                            break;

                        case 6:
                            List<Student> studentsWithScores = scoreManager.GetStudentsWithScores();
                            Console.WriteLine("Список студентов с оценками:");
                            foreach (var student in studentsWithScores)
                            {
                                Console.WriteLine($"Оценки {student.Name}:");
                                foreach (var item in student.GetScores())
                                {
                                    Console.WriteLine($"{item.Key}: {item.Value}");
                                }
                                Console.WriteLine();
                            }
                            break;

                        case 7:
                            Console.WriteLine("Сортировка студентов по имени:");
                            scoreManager.Sort((s1, s2) => string.Compare(s1.Name, s2.Name, StringComparison.OrdinalIgnoreCase));
                            break;

                        case 8:
                            Console.Write("Введите предмет для фильтрации по предмету (от большей к меньшей оценке): ");
                            string filterSubject = Console.ReadLine();
                            List<Student> filteredStudents = scoreManager.Filter(student => student.GetScores().ContainsKey(filterSubject));
                            filteredStudents.Sort((s1, s2) => s2.GetScore(filterSubject).CompareTo(s1.GetScore(filterSubject)));
                            scoreManager.DisplayStudents($"Отфильтрованные студенты по предмету {filterSubject} (от большей к меньшей оценке):", filteredStudents);
                            break;

                        case 9:
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("Неверный выбор. Пожалуйста, выберите правильную опцию.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Пожалуйста, введите корректное число.");
                }
            }
        }
    }
}