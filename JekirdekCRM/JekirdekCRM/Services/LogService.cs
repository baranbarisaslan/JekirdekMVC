using JekirdekCRM.Helpers;
using JekirdekCRM.Models.DBModels;
using JekirdekCRM.Repositories;

namespace JekirdekCRM.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepo;

        public LogService(ILogRepository logRepo)
        {
            _logRepo = logRepo;
        }

        public async Task CreateLogAsync(LogTags tag, int userId)
        {
            var log = new Log
            {
                CreatedAt = DateTime.UtcNow,
                RelatedUserId = userId,
                Tag = tag.ToString()
            };

            await _logRepo.CreateLogAsync(log);
        }

        public async Task CreateLogAsync(LogTags tag, string text)
        {
            var log = new Log
            {
                CreatedAt = DateTime.UtcNow,
                Tag = tag.ToString(),
                Text = text
            };

            await _logRepo.CreateLogAsync(log);
        }

        public async Task CreateLogAsync(LogTags tag, string text, int userId)
        {
            var log = new Log
            {
                CreatedAt = DateTime.UtcNow,
                Tag = tag.ToString(),
                Text = text,
                RelatedUserId = userId
            };

            await _logRepo.CreateLogAsync(log);
        }
    }
}
