using JekirdekCRM.Models.DBModels;
using JekirdekCRM.Helpers;

namespace JekirdekCRM.Repositories
{
    public interface ILogRepository
    {
        Task AddAsync(Log log);
    }

}
