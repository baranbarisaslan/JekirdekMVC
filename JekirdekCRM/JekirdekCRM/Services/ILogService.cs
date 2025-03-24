using JekirdekCRM.Helpers;
using JekirdekCRM.Models.DBModels;

namespace JekirdekCRM.Services
{
    public interface ILogService
    {
        Task CreateLogAsync(LogTags tag, int userId);
        Task CreateLogAsync(LogTags tag, string text);
        Task CreateLogAsync(LogTags tag, string text, int userId);
    }
}
