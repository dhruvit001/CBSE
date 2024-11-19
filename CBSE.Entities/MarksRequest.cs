
namespace CBSE.Entities
{
    public class MarksRequest
    {
        public int StudentId { get; set; }

        // Marks for each subject
        public decimal Math { get; set; }
        public decimal Science { get; set; }
        public decimal English { get; set; }
        public decimal History { get; set; }
        public decimal Geography { get; set; }
    }
}
