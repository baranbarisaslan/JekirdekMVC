using JekirdekCRM.Models;
using JekirdekCRM.Models.DBModels;

namespace JekirdekCRM.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly DatabaseContext _context;

        public LogRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Log log)
        {
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

    }
}
