using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Students.Models
{
    public class Student : TableEntity
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Faculty { get; set; }
        public int Year { get; set; }

        public Student(string lastName, string firstName, string faculty, int year, string id = null)
        {
            if (String.IsNullOrEmpty(id))
            {
                Id = Guid.NewGuid();
            }
            else
            {
                Id = Guid.Empty;
            }
            LastName = lastName;
            FirstName = firstName;
            Faculty = faculty;
            Year = year;
        }

        public Student(Student student)
        {
            Id = Guid.NewGuid();
            this.LastName = student.LastName;
            this.Faculty = student.Faculty;
            this.FirstName = student.FirstName;
            this.Year = student.Year;
        }

        public Student()
        {
            Id = Guid.NewGuid();
        }
    }
}
