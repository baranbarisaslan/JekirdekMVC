using JekirdekCRM.Models;
using JekirdekCRM.Models.DBModels;
using JekirdekCRM.Models.ViewModels;

namespace JekirdekCRM.Services
{
    public interface ICustomerService
    {
        ServiceResult AddCustomer(CustomerViewModel model);

        (List<Customer> Customers, int TotalCount) ListCustomers(string? search, int page, int pageSize);
    }
}
