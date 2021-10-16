using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Students.Services
{
    public class StudentsRepo
    {
        private System.Collections.Generic.IEnumerable<Students.Models.Student> AllStudents;

        public StudentsRepo()
        {
            AllStudents = Enumerable.Empty<Students.Models.Student>();
            AllStudents = AllStudents.Append(new Models.Student("Pop", "Ion", "Automatica si Calculatoare", 3));
            AllStudents = AllStudents.Append(new Models.Student("Popescu", "George", "Mecanica", 2));
            AllStudents = AllStudents.Append(new Models.Student("Ardelean", "Vasile", "Management si Marketing", 1));
            AllStudents = AllStudents.Append(new Models.Student("Ionescu", "Vlad", "Educatie Fizica si Sport", 3));
            AllStudents = AllStudents.Append(new Models.Student("Kali", "Gabriel", "Automatica si Calculatoare", 4));
        }

        public IEnumerable<Students.Models.Student> returnStudents()
        {
            // AllStudents = AllStudents.Append(new Models.Student("Kali", "Gabriel", "Automatica si Calculatoare", 4, "ok"));
            return AllStudents;
        }

        public Students.Models.Student returnStudent(string id)
        {
            var studentExists = AllStudents.FirstOrDefault(s => s.Id.ToString() == id);
            return studentExists;
        }

        public bool checkStudent(Students.Models.Student student)
        {
            var studentExists = AllStudents.Any(
                s => s.LastName == student.LastName && s.FirstName == student.FirstName && s.Faculty == student.Faculty && s.Year == student.Year
            );
            if(studentExists)
            {
                return true;
            }
            return false;
        }

        public bool checkId(string id)
        {
            var studentExists = AllStudents.Any(
                s => s.Id.ToString() == id
            );
            if(studentExists)
            {
                return true;
            }
            return false;
        }

        public void addStudent(Students.Models.Student student)
        {
            AllStudents = AllStudents.Append(student);
        }

        public void updateStudent(string id, Students.Models.Student student)
        {
            // because when a student object is created from the received body of a request,
            // the new student has a newly assigned Id but we have to leave it unchanged
            student.Id = Guid.Parse(id); // we are sure this is a valid Guid because the update occurs only when the student is identified
            // updating the enumerable element
            List<Students.Models.Student> studentsList = new List<Students.Models.Student>();
            studentsList = AllStudents.ToList();
            studentsList[studentsList.FindIndex(s => s.Id.ToString() == id)] = student;
            AllStudents = studentsList.AsEnumerable();
        }

        public Students.Models.Student deleteStudent(string id)
        {
            // removing the enumerable element
            List<Students.Models.Student> studentsList = new List<Students.Models.Student>();
            studentsList = AllStudents.ToList();
            Students.Models.Student student = studentsList.First(s => s.Id.ToString() == id);
            studentsList.Remove(student);
            AllStudents = studentsList.AsEnumerable();
            return student;
        }
    }
}
