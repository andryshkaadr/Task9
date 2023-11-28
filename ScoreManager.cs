namespace Task9
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class ScoreManager
    {
        private Dictionary<string, Student> Students { get; set; }

        public ScoreManager()
        {
            Students = new Dictionary<string, Student>();
        }

        public delegate int ComparisonDelegate(Student student1, Student student2);

        public void AddStudent(string name, Dictionary<string, int> scores)
        {
            if (!Students.ContainsKey(name))
            {
                var student = new Student(name);
                foreach (var subject in scores.Keys)
                {
                    student.AddScore(subject, scores[subject]);
                }
                Students[name] = student;
            }
        }

        public void RemoveStudent(string name)
        {
            if (Students.ContainsKey(name))
            {
                Students.Remove(name);
            }
        }

        public void AddScore(string studentName, string subject, int score)
        {
            if (Students.ContainsKey(studentName))
            {
                Students[studentName].AddScore(subject, score);
            }
        }

        public void RemoveScore(string studentName, string subject)
        {
            if (Students.ContainsKey(studentName))
            {
                Students[studentName].RemoveScore(subject);
            }
        }

        public int GetStudentScore(string studentName, string subject)
        {
            return Students.ContainsKey(studentName) ? Students[studentName].GetScore(subject) : -1;
        }

        public List<Student> GetStudentsWithScores()
        {
            return new List<Student>(Students.Values);
        }

        public void Sort(ComparisonDelegate comparison)
        {
            List<Student> sortedStudents = Students.Values.OrderBy(s => s, new StudentComparer(comparison)).ToList();
            DisplayStudents("Отсортированные студенты:", sortedStudents);
        }

        public List<Student> Filter(Func<Student, bool> filter)
        {
            return Students.Values.Where(filter).ToList();
        }

        public void DisplayStudents(string message, List<Student> students)
        {
            Console.WriteLine(message);
            foreach (var student in students)
            {
                Console.WriteLine($"{student.Name}'s Scores:");
                foreach (var item in student.GetScores())
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }
                Console.WriteLine();
            }
        }

        public void SaveData()
        {
            DataSerializer.SaveBinary(GetStudentsWithScores(), "students.bin");
            DataSerializer.SaveJson(GetStudentsWithScores(), "students.json");
            Console.WriteLine("Данные сохранены в файлах (Binary и JSON).");
        }

        public void LoadData()
        {
            ScoreManager manager = DataSerializer.LoadJson<ScoreManager>("students.json");

            if (manager != null)
            {
                Students = manager.Students;
            }
            else
            {
                List<Student> students = DataSerializer.LoadBinary<List<Student>>("students.bin");
                if (students != null && students.Count > 0)
                {
                    foreach (var student in students)
                    {
                        Students[student.Name] = student;
                    }
                }
            }
        }
    }
}