using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Students.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {

        private readonly Services.StudentsRepo _students;

        public StudentsController(Services.StudentsRepo repo)
        {
            this._students = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
            => Ok(_students.returnStudents());

        // did a LOT of research to understand how to use lambda expressions like this
        // I just wanted to use multiple statements in the input parameters of a lambda expression like THERE
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById([FromRoute] string id)
            => ((Func<IActionResult>)( // over engineering at its finest...
                    () => 
                    {
                        // like HERE
                        var s =_students.returnStudent(id);
                        switch (s)
                        {
                            case null:
                                return NotFound();
                                break;
                            default: 
                                return Ok(s);
                                break;
                        };
                    }
                ))();

        [HttpPost]
        public async Task<IActionResult> AddNewStudent([FromBody] Students.Models.Student studentBody)
            => !(_students.checkStudent(studentBody)) switch
            {
                true => ((Func<IActionResult>)(() => 
                {
                    // or HERE
                    _students.addStudent(studentBody);
                    return CreatedAtAction(nameof(GetStudentById), new {id = studentBody.Id}, studentBody);
                }))(),
                false => BadRequest("Student already exists.")
            };
    }
}
