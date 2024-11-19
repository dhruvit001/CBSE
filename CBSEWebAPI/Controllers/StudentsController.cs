using CBSE.Entities;
using CBSE.Service.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CBSEWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]

    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("{studentId}")]
        [Authorize]
        public async Task<IActionResult> GetStudentById(int studentId)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(studentId);
                if (student == null)
                {
                    return NotFound();
                }
                return Ok(student);
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString());   
            }
            return BadRequest();
            
            
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetStudents([FromQuery] int schoolId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var students = await _studentService.GetStudentsBySchoolIdAsync(schoolId, pageNumber, pageSize);
            return Ok(students);
        }

        [HttpGet("{studentsId}")]
        [Authorize]
        public async Task<IActionResult> GetStudentMarks(int studentsId)
        {
            var schoolIdClaim = User.FindFirst("SchoolId");
            if (schoolIdClaim == null)
            {
                return Unauthorized("SchoolId not found in token.");
            }
            // Parse the SchoolId from the claim
            if (!int.TryParse(schoolIdClaim.Value, out var schoolId))
            {
                return Unauthorized("Invalid SchoolId format.");
            }
            var students = await _studentService.GetStudentMarks(studentsId, schoolId);
            return Ok(students);
        }
        
        [HttpPost("enroll")]
        [Authorize]
        public async Task<IActionResult> EnrollStudent([FromBody] EnrollStudentRequest model)
        {
            var schoolIdClaim = User.FindFirst("SchoolId");
            if (schoolIdClaim == null)
            {
                return Unauthorized("SchoolId not found in token.");
            }
            // Parse the SchoolId from the claim
            if (!int.TryParse(schoolIdClaim.Value, out var schoolId))
            {
                return Unauthorized("Invalid SchoolId format.");
            }
            var response = await _studentService.EnrollStudentAsync(model.Name, model.Age, schoolId);
            return Ok(response);
            //return CreatedAtAction(nameof(GetStudentById), new { studentId = student.StudentId }, student);
        }

        [HttpPost]
        [Authorize(Roles = "ROOT")]
        public async Task<IActionResult> AddMarks(MarksRequest model)
        {
            var res = await _studentService.AddMarksAsync(model);
            return Ok(res);
        }
    }
}
