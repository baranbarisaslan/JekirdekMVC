using JekirdekCRM.Models;
using JekirdekCRM.Models.ViewModels;

namespace JekirdekCRM.Services
{
    public interface ICustomerService
    {
        ServiceResult AddCustomer(CustomerViewModel model);
    }
}
