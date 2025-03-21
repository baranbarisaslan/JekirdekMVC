using Microsoft.EntityFrameworkCore;

namespace JekirdekCRM.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {


        }
    }
}
