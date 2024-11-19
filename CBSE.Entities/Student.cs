using System.Text.Json.Serialization;

namespace CBSE.Entities
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string RollNo { get; set; }
        public int Age { get; set; }
        public int SchoolId { get; set; } // Foreign Key

        [JsonIgnore]
        public School School { get; set; }
        [JsonIgnore]
        public ICollection<Marks> Marks { get; set; }
    }

}
