
namespace CBSE.Entities
{
    public class School
    {
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public ICollection<Student> Students { get; set; }
    }

}
