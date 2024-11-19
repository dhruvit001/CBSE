using System.Data;

namespace CBSE.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // In a real-world scenario, store this as a hashed password
        public int SchoolId { get; set; } // Each user belongs to a specific school
        public int RoleId { get; set; }
        public Roles Roles { get; set; }

    }
}
