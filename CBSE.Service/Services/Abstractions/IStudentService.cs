

using CBSE.Entities;

namespace CBSE.Service.Services.Abstractions
{
    public interface IStudentService
    {
        Task<Student> GetStudentByIdAsync(int studentId);
        Task<IEnumerable<Student>> GetStudentsBySchoolIdAsync(int schoolId, int pageNumber, int pageSize);
        Task<BaseResponse> EnrollStudentAsync(string name, int age, int schoolId);
        Task<BaseResponse> AddMarksAsync(MarksRequest marks);
        Task<IEnumerable<Marks>> GetStudentMarks(int studentId, int schoolId);
    }
}
