using CBSE.Data.Repositories.Abstractions;
using CBSE.Entities;
using CBSE.Service.Services.Abstractions;


namespace CBSE.Service.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _studentRepository.GetStudentByIdAsync(studentId);
        }

        public async Task<IEnumerable<Student>> GetStudentsBySchoolIdAsync(int schoolId, int pageNumber, int pageSize)
        {
            return await _studentRepository.GetStudentsBySchoolIdAsync(schoolId, pageNumber, pageSize);
        }

        public async Task<BaseResponse> EnrollStudentAsync(string name, int age, int schoolId)
        {
            return await _studentRepository.EnrollStudentAsync(name, age, schoolId);
        }

        public async Task<BaseResponse> AddMarksAsync(MarksRequest marks)
        {
            return await _studentRepository.AddMarksAsync(marks);
        }

        public async Task<IEnumerable<Marks>> GetStudentMarks(int studentId, int schoolId)
        {


            return await _studentRepository.GetStudentMarks(studentId, schoolId);
        }
    }
}
