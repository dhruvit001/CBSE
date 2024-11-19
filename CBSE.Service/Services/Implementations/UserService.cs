using CBSE.Data.Repositories.Abstractions;
using CBSE.Entities;
using CBSE.Service.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSE.Service.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private readonly List<User> _users;

        //public UserService()
        //{
        //    // Sample users in-memory (You can replace this with a database call)
        //    _users = new List<User>
        //{
        //    new User { Id = 1, Username = "principal1", Password = "password123", SchoolId = 101 },
        //    new User { Id = 2, Username = "principal2", Password = "password456", SchoolId = 102 },
        //    new User { Id = 3, Username = "string", Password = "string", SchoolId = 103 }
        //};
        //}

        // Simulating user validation (password matching in a real-world scenario should be done with hashed passwords)
        public User ValidateUser(string username, string password)
        {
            return _userRepository.ValidateUser(username, password);
            //return _users.SingleOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
