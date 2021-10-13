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
            AllStudents = AllStudents.Append(new Models.Student( "Pop", "Ion", "Automatica si Calculatoare", 3));
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
            var idExists = AllStudents.FirstOrDefault(s => s.Id.ToString() == id);
            return idExists;
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
        public IEnumerable<Students.Models.Student> addStudent(Students.Models.Student student)
        {
            AllStudents = AllStudents.Append(student);
            return AllStudents;
        }
    }
}
