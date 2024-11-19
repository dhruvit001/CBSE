using CBSE.Data.Repositories.Abstractions;
using CBSE.Entities;
using Microsoft.EntityFrameworkCore;

namespace CBSE.Data.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public User ValidateUser(string username, string password)
        {
            var user = _context.Users.AsNoTracking()
            .Include(u => u.Roles)  // Include the Role table in the query
            .SingleOrDefaultAsync(u => u.Username == username);

            return user.Result;
        }
    }
}
