using JekirdekCRM.Models.DBModels;
using Microsoft.EntityFrameworkCore;

namespace JekirdekCRM.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {


        }

        public DbSet<User> Users { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Log> Logs { get; set; }

    }
}
