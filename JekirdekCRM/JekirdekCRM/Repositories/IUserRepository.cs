using JekirdekCRM.Models.DBModels;

namespace JekirdekCRM.Repositories
{
    public interface IUserRepository
    {
        void AddUser(User user);
        Task<User?> GetUserByUsername(string username);
    }
}
