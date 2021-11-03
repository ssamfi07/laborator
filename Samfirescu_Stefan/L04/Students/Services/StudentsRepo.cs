using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Extensions.Configuration;

namespace Students.Services
{
    public class StudentsRepo
    {
        private System.Collections.Generic.IEnumerable<Students.Models.Student> AllStudents;
         private CloudTableClient _tableClient;
        private CloudTable _studentsTable;
        private string _connectionString;
        public StudentsRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue(typeof(string), "AzureStorageAccountConnectionString").ToString();
            Task.Run(async () => { await InitializeTable(); })
                .GetAwaiter()
                .GetResult();
        }

        public List<Students.Models.Student> returnStudents()
        {
            var students = new List<Students.Models.Student>();

            TableQuery<Students.Models.Student> query = new TableQuery<Students.Models.Student>();
            TableContinuationToken token = null;
            do
            {
                var resultSegment = _studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                students.AddRange(resultSegment.Results);
            } while (token != null);
            return students;
        }

        public Students.Models.Student returnStudent(string id)
        {
            var rowKey = id;

            var query = TableOperation.Retrieve<Students.Models.Student>("", rowKey);
            var result = _studentsTable.ExecuteAsync(query);
            return (Students.Models.Student)result.Result;
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
            var insertOperation = TableOperation.Insert(student);
            var result = _studentsTable.ExecuteAsync(insertOperation);
            // return (Students.Models.Student)result.Result;
        }

        public void updateStudent(string id, Students.Models.Student student)
        {
            var editOperation = TableOperation.Merge(student);
            var result = _studentsTable.ExecuteAsync(editOperation);
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

        private async Task InitializeTable()
        {
            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();
            _studentsTable = _tableClient.GetTableReference("studenti");

            await _studentsTable.CreateIfNotExistsAsync();
        }
    }
}
