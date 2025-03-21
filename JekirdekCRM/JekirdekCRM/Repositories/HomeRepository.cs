using JekirdekCRM.Helpers;
using JekirdekCRM.Models;
using JekirdekCRM.Models.DBModels;
using Microsoft.EntityFrameworkCore;

namespace JekirdekCRM.Repositories
{
    public class HomeRepository
    {
        private readonly DatabaseContext context;

        public HomeRepository(DatabaseContext _context)
        {
            context = _context;
        }


        public void CreateLog(LogTags logtag, int UserId)
        {
            Log log = new Log() { CreatedAt = DateTime.UtcNow, RelatedUserId = UserId, Tag = logtag.ToString()};
            context.Logs.Add(log);
            context.SaveChanges();
        }

        public void CreateLog(LogTags logtag, string text)
        {
            Log log = new Log() { CreatedAt = DateTime.UtcNow, Tag = logtag.ToString(), Text = text };
            context.Logs.Add(log);
            context.SaveChanges();
        }

        public void CreateLog(LogTags logtag, string text, int UserId)
        {
            Log log = new Log() { CreatedAt = DateTime.UtcNow, RelatedUserId = UserId, Tag = logtag.ToString(), Text = text };
            context.Logs.Add(log);
            context.SaveChanges();
        }



        public User CreateUser(string username, string password, string phonenumber, string Role)
        {
            string salt = ManageHashing.CreateSalt();
            string pwd = ManageHashing.SHA256(password, salt);
            User user = new User()
            {
                Username = username,
                Password = pwd,
                Salt = salt,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PhoneNumber = phonenumber,
                Role = Role,

            };
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }
    }
}
