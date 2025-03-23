using JekirdekCRM.Helpers;
using JekirdekCRM.Models.DBModels;
using JekirdekCRM.Repositories;
using System.Threading.Tasks;

namespace JekirdekCRM.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogRepository _logRepo;

        public UserService(IUserRepository userRepo, ILogRepository logRepo)
        {
            _userRepo = userRepo;
            _logRepo = logRepo;
        }

        public User CreateUser(string username, string password, string phonenumber, string role)
        {
            string salt = ManageHashing.CreateSalt();
            string hashedPassword = ManageHashing.SHA256(password, salt);

            var user = new User
            {
                Username = username,
                Password = hashedPassword,
                Salt = salt,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PhoneNumber = phonenumber,
                Role = role
            };

            _userRepo.AddUser(user);

            var log = new Log
            {
                CreatedAt = DateTime.UtcNow,
                RelatedUserId = user.Id,
                Tag = LogTags.DatabaseAction.ToString(),
                Text = $"Yeni kullanıcı oluşturuldu: {username}"
            };

            _logRepo.CreateLogAsync(log);

            return user;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _userRepo.GetUserByUsername(username);
        }

        public bool VerifyPassword(User user, string password)
        {
            string hashed = ManageHashing.SHA256(password, user.Salt);
            return hashed == user.Password;
        }
    }
}
