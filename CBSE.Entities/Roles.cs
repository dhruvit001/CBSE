using System.ComponentModel.DataAnnotations;

namespace CBSE.Entities
{
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }
        public string Rolename { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();    
    }
}
