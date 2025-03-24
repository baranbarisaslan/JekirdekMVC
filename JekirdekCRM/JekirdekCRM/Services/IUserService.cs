using JekirdekCRM.Models.DBModels;

namespace JekirdekCRM.Services
{
    public interface IUserService
    {
        User CreateUser(string username, string password, string phonenumber, string role);
        Task<User>? GetUserByUsername(string username);
        bool VerifyPassword(User user, string password);
    }
}
