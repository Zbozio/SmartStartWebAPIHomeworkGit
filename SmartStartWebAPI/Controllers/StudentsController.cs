using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using SmartStartWebAPI.Domain;
using SmartStartWebAPI.DTOs;
using SmartStartWebAPI.Infrastructure;

namespace SmartStartWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private StudentsDBContext _dbContext;
        public StudentsController(StudentsDBContext dbContext)
        {
            _dbContext = dbContext;



        }
        [HttpGet]
        public IEnumerable<StudentNamesDto> GetStudentsNames()
        {
            var students = _dbContext.Students.ToList();
            return students.Select(x => new StudentNamesDto
            {
                Id = x.StudentId,
                Name = x.Name,
                LastName = x.LastName
            });
        }
        [HttpGet("{id}")]
        public ActionResult<StudentDto> GetStudent([FromRoute] int id)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.StudentId == id);
            if (student == null)
                return NotFound();
            var studentDto = new StudentDto
            {
                Id = student.StudentId,
                Name = student.Name,
                LastName = student.LastName,
                Email = student.Email,
                Age = student.Age,
                IsActive = student.IsActive
            };
            return Ok(studentDto);
        }
        [HttpGet("average-age")]
        public int GetStudentsAverageAge()
        {
            var students = _dbContext.Students.ToList();
            var studentsCount = students.Count();
            var sumAge = students.Sum(x => x.Age);
            return sumAge / studentsCount;
        }
        [HttpGet("oldest-age")]
        public ActionResult GetStudentsOldestAge()
        {
            {

                var students = _dbContext.Students.ToList();

                var orderByResult = from s in students
                                    orderby s.Age descending
                                    select s;
                var order = orderByResult.Take(1);

                return Ok(order);
            }
        }


        [HttpGet("youngest-age")]
        public ActionResult GetStudentsYoungestAge()
        {

            var students = _dbContext.Students.ToList();

            var orderByResult = from s in students
                                orderby s.Age
                                select s;
            var order = orderByResult.Take(1);

            return Ok(order);
        }
        [HttpGet("aboveaverage-age")]
        public ActionResult GetStudentsAboveAverageAge()
        {
            var students = _dbContext.Students.ToList();
            var studentsCount = students.Count();
            var sumAge = students.Sum(x => x.Age);
            var average = sumAge / studentsCount;

           var above= students.FindAll(x => x.Age > average);
            return Ok(above);

           }

        [HttpPost]
        public ActionResult<StudentDto> AddStudent([FromBody] StudentToCreateDto studentToCreateDto)
        {
            var student = new Student
            {
                Name = studentToCreateDto.Name,
                LastName = studentToCreateDto.LastName,
                Email = studentToCreateDto.Email,
                Age = studentToCreateDto.Age,
                IsActive = studentToCreateDto.IsActive
            };
            _dbContext.Students.Add(student);
            _dbContext.SaveChanges();
            var studentDto = new StudentDto
            {
                Id = student.StudentId,
                Name = student.Name,
                LastName = student.LastName,
                Email = student.Email,
                Age = student.Age,
                IsActive = student.IsActive
            };
            return Created(string.Empty, studentDto);
        }


        [HttpPut("{id}")]
        public ActionResult UpdateStudent([FromRoute] int id, [FromBody] StudentToUpdateDto studentToUpdateDto)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.StudentId == id);
            if (student == null)
                return NotFound();
            student.Name = studentToUpdateDto.Name;
            student.LastName = studentToUpdateDto.LastName;
            student.Email = studentToUpdateDto.Email;
            student.Age = studentToUpdateDto.Age;
            _dbContext.SaveChanges();
            return NoContent();
        }
        [HttpPut("Change_Student_Status")]
        public ActionResult ChangeActiveStatus(int id)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.StudentId == id);
            student.IsActive = !student.IsActive;
            _dbContext.SaveChanges();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public ActionResult Deletetudent([FromRoute] int id)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.StudentId == id);
            if (student == null)
                return NotFound();
            _dbContext.Students.Remove(student);
            _dbContext.SaveChanges();
            return NoContent();
        }

    }
}
