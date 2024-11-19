using CBSE.Entities;


namespace CBSE.Service.Services.Abstractions
{
    public interface IUserService
    {
        User ValidateUser(string username, string password);
    }
}
