using System.Text.Json.Serialization;

namespace CBSE.Entities
{
    public class Marks
    {
        public int MarksId { get; set; }
        public int StudentId { get; set; } // Foreign Key

        // Marks for each subject
        public decimal Math { get; set; }
        public decimal Science { get; set; }
        public decimal English { get; set; }
        public decimal History { get; set; }
        public decimal Geography { get; set; }
        [JsonIgnore]
        public Student Student { get; set; }
    }

}
