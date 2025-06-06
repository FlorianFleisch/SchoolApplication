using Microsoft.AspNetCore.Mvc;
using _3aWI_Projekt.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using _3aWI_Projekt.DTO;
using _3aWI_Projekt.Models;

namespace _3aWI_Projekt.Controllers
{
    [ApiController]
    [Route("api")]
    public class SchoolController : ControllerBase
    {
        private readonly AppDbContext _Context;

        public SchoolController(AppDbContext context)
        {
            _Context = context;
        }

        [HttpPost("schools")]
        public IActionResult CreateSchool([FromBody] SchoolDto request)
        {
            var school = new School(request.Name);
            _Context.Schools.Add(school);
            _Context.SaveChanges();
            return Created($"/api/schools/{school.ID}", new { id = school.ID });
        }

        [HttpPost("students")]
        public IActionResult CreateStudent([FromBody] StudentDto dto)
        {
            var student = new Student(dto.Firstname, dto.Lastname, dto.Gender, dto.Birthdate, dto.SchoolClass, dto.Track);
            _Context.Students.Add(student);
            _Context.SaveChanges();
            return Created($"/api/students/{student.ID}", new { id = student.ID });
        }

        [HttpPost("classrooms")]
        public IActionResult CreateClassroom([FromBody] ClassroomDto dto)
        {
            var room = new Classroom(dto.Name, dto.Size, dto.Seats, dto.Cynap);
            _Context.Classrooms.Add(room);
            _Context.SaveChanges();
            return Created($"/api/classrooms/{room.ID}", new { id = room.ID });
        }

        [HttpGet("schools")]
        public IActionResult GetSchools()
        {
            return Ok(_Context.Schools.Select(s => new { s.ID, s.Name }));
        }

        [HttpGet("students/count")]
        public IActionResult GetNumberOfStudents()
        {
            int count = _Context.Students.Count();
            return Ok(count);
        }

        [HttpGet("students/gender-count")]
        public IActionResult GetMaleAndFemaleStudentCount()
        {
            int male = _Context.Students.Count(s => s.Gender == Person.Genders.m);
            int female = _Context.Students.Count(s => s.Gender == Person.Genders.w);
            return Ok(new { male, female });
        }

        [HttpGet("classrooms/count")]
        public IActionResult GetNumberOfClassrooms()
        {
            int count = _Context.Classrooms.Count();
            return Ok(count);
        }

        [HttpGet("students/average-age")]
        public IActionResult GetAverageAge()
        {
            double avg = _Context.Students.Any() ? _Context.Students.Average(s => s.Age) : 0;
            return Ok(avg);
        }

        [HttpGet("classrooms/with-cynap")]
        public IActionResult GetClassroomsWithCynap()
        {
            var rooms = _Context.Classrooms.Where(c => c.Cynap)
                .Select(r => new { r.ID, r.Name });
            return Ok(rooms);
        }

        [HttpGet("classes/count")]
        public IActionResult GetNumberOfClasses()
        {
            int count = _Context.Students.Select(s => s.SchoolClass).Distinct().Count();
            return Ok(count);
        }

        [HttpGet("classes/student-counts")]
        public IActionResult GetClassStudentCounts()
        {
            var result = _Context.Students
                .GroupBy(s => s.SchoolClass.ToString())
                .ToDictionary(g => g.Key, g => g.Count());
            return Ok(result);
        }

        [HttpGet("classes/{className}/female-percentage")]
        public IActionResult GetFemalePercentageInClass(string className)
        {
            var students = _Context.Students
                .Where(s => s.SchoolClass.ToString() == className);
            int total = students.Count();
            if (total == 0) return Ok(0);
            int female = students.Count(s => s.Gender == Person.Genders.w);
            double percentage = (double)female / total * 100;
            return Ok(percentage);
        }

        [HttpGet("classes/{className}/can-fit/{roomId}")]
        public IActionResult CanClassFitInRoom(string className, int roomId)
        {
            var room = _Context.Classrooms.Find(roomId);
            if (room == null) return NotFound();
            int size = _Context.Students.Count(s => s.SchoolClass.ToString() == className);
            return Ok(room.Seats >= size);
        }

        [HttpPost("schools/{schoolId}/students/{studentId}")]
        public IActionResult AddStudentToSchool(int schoolId, int studentId)
        {
            var school = _Context.Schools.Include(s => s.Students).FirstOrDefault(s => s.ID == schoolId);
            var student = _Context.Students.Find(studentId);
            if (school == null || student == null) return NotFound();
            school.AddStudent(student);
            _Context.SaveChanges();
            return Ok();
        }

        [HttpPost("schools/{schoolId}/classrooms/{classroomId}")]
        public IActionResult AddClassroomToSchool(int schoolId, int classroomId)
        {
            var school = _Context.Schools.Include(s => s.Classrooms).FirstOrDefault(s => s.ID == schoolId);
            var room = _Context.Classrooms.Find(classroomId);
            if (school == null || room == null) return NotFound();
            school.AddClassroom(room);
            _Context.SaveChanges();
            return Ok();
        }

        [HttpPost("classrooms/{classroomId}/students/{studentId}")]
        public IActionResult AddStudentToClassroom(int classroomId, int studentId)
        {
            var room = _Context.Classrooms.Include(r => r.Students).FirstOrDefault(r => r.ID == classroomId);
            var student = _Context.Students.Find(studentId);
            if (room == null || student == null) return NotFound();
            room.AddStudent(student);
            _Context.SaveChanges();
            return Ok();
        }

        [HttpGet("schools/{schoolId}/values")]
        public IActionResult GetSchoolValues(int schoolId)
        {
            var school = _Context.Schools
                .Include(s => s.Students)
                .Include(s => s.Classrooms)
                .FirstOrDefault(s => s.ID == schoolId);
            if (school == null) return NotFound();

            var (male, female) = school.GetMaleAndFemaleStudentCount();
            var result = new
            {
                numberOfStudents = school.GetNumberOfStudents(),
                numberOfMaleStudents = male,
                numberOfFemaleStudents = female,
                averageAgeOfStudents = school.GetAverageAge(),
                numberOfClassrooms = school.GetNumberOfClassrooms(),
                classroomsWithCynap = school.GetClassroomsWithCynap().Select(c => c.Name),
                classroomsWithNumberOfStudents = school.GetClassStudentCounts()
            };

            return Ok(result);
        }

        [HttpGet("schools/{schoolId}/classes/{className}/female-percentage")]
        public IActionResult GetFemalePercentageInClass(int schoolId, string className)
        {
            var school = _Context.Schools.Include(s => s.Students).FirstOrDefault(s => s.ID == schoolId);
            if (school == null) return NotFound();
            double percentage = school.GetFemalePercentageInClass(className);
            return Ok(percentage);
        }

        [HttpGet("schools/{schoolId}/classrooms/{roomId}/can-fit/{className}")]
        public IActionResult CanClassFitInRoom(int schoolId, int roomId, string className)
        {
            var school = _Context.Schools
                .Include(s => s.Students)
                .Include(s => s.Classrooms)
                .FirstOrDefault(s => s.ID == schoolId);
            if (school == null) return NotFound();
            var room = school.Classrooms.FirstOrDefault(r => r.ID == roomId);
            if (room == null) return NotFound();
            return Ok(school.CanClassFitInRoom(className, room));
        }
    }
}

