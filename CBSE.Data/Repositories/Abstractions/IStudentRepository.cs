using CBSE.Entities;


namespace CBSE.Data.Repositories.Abstractions
{
    public interface IStudentRepository
    {
        Task<Student> GetStudentByIdAsync(int studentId);
        Task<IEnumerable<Student>> GetStudentsBySchoolIdAsync(int schoolId, int pageNumber, int pageSize);
        Task<BaseResponse> EnrollStudentAsync(string name, int age, int schoolId);
        Task<BaseResponse> AddMarksAsync(MarksRequest marks);
        Task<IEnumerable<Marks>> GetStudentMarks(int studentId, int schoolId);
    }
}
