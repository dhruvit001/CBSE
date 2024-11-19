using CBSE.Data.Repositories.Abstractions;
using CBSE.Entities;
using Microsoft.EntityFrameworkCore;

namespace CBSE.Data.Repositories.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _context.Students
                                 .Include(s => s.Marks)
                                 .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        public async Task<IEnumerable<Student>> GetStudentsBySchoolIdAsync(int schoolId, int pageNumber, int pageSize)
        {
            return await _context.Students
                                 .Where(s => s.SchoolId == schoolId)
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public async Task<BaseResponse> EnrollStudentAsync(string name, int age, int schoolId)
        {
            var student = new Student { Name = name, Age = age, SchoolId = schoolId, RollNo = await GenerateRollNumberAsync(schoolId) };

            try
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    Success = true,
                    Message = "Student enrolled successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = $"Error occurred while enrolling student: {ex.Message}"
                };
            }
        }

        private async Task<string> GenerateRollNumberAsync(int schoolId)
        {
            var currentYear = DateTime.Now.Year;

            var lastStudent = await _context.Students
                                            .Where(s => s.SchoolId == schoolId && s.RollNo.StartsWith($"{schoolId}-{currentYear}"))
                                            .OrderByDescending(s => s.StudentId) 
                                            .FirstOrDefaultAsync();

            var nextSeq = lastStudent == null ? 1 : int.Parse(lastStudent.RollNo.Split('-').Last()) + 1;

            var rollNo = $"{schoolId}-{currentYear}-{nextSeq:000}";  

            return rollNo;
        }

        public async Task<BaseResponse> AddMarksAsync(MarksRequest marks)
        {
            BaseResponse response = new BaseResponse();
            if (marks.Math < 0 || marks.Math > 100)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Invalid Math marks."
                };

            if (marks.Science < 0 || marks.Science > 100)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Invalid Science marks."
                };

            if (marks.English < 0 || marks.English > 100)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Invalid English marks."
                };

            if (marks.History < 0 || marks.History > 100)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Invalid History marks."
                };

            if (marks.Geography < 0 || marks.Geography > 100)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Invalid Geography marks."
                };

            var studentMarks = await _context.Marks.FirstOrDefaultAsync(m => m.StudentId == marks.StudentId);
            if (studentMarks != null)
            {
                studentMarks.Math = marks.Math;
                studentMarks.Science = marks.Science;
                studentMarks.English = marks.English;
                studentMarks.History = marks.History;
                studentMarks.Geography = marks.Geography;
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    Success = true,
                    Message = "Marks updated successfully."
                };

            }
            else
            {
                var StudentMarks = new Marks
                {
                    Science = marks.Science,
                    English = marks.English,
                    Math = marks.Math,
                    Geography = marks.Geography,
                    StudentId = marks.StudentId,
                    History = marks.History,
                };

                _context.Marks.Add(StudentMarks);
                await _context.SaveChangesAsync();
                return new BaseResponse
                {
                    Success = true,
                    Message = "Marks added successfully."
                };
            }
        }

        public async Task<IEnumerable<Marks>> GetStudentMarks(int studentId, int schoolId)
        {
            var marks = await _context.Marks.Include(a => a.Student)
                                           .Where(s => s.StudentId == studentId && s.Student.SchoolId == schoolId)
                                           .ToListAsync();

            if (!marks.Any())
            {
                throw new InvalidOperationException($"No marks found for student with ID {studentId} in school with ID {schoolId}");
            }

            return marks;
        }
    }
}



