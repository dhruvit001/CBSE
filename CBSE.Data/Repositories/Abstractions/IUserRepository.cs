using CBSE.Entities;

namespace CBSE.Data.Repositories.Abstractions
{
    public interface IUserRepository
    {
        User ValidateUser(string username, string password);
    }
}
