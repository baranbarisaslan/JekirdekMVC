using JekirdekCRM.Models;
using JekirdekCRM.Models.DBModels;
using JekirdekCRM.Models.ViewModels;
using JekirdekCRM.Repositories;
using JekirdekCRM.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepo;
    private readonly ILogService _logService;

    public CustomerService(ICustomerRepository customerRepo, ILogService logService)
    {
        _customerRepo = customerRepo;
        _logService = logService;
    }

    public ServiceResult AddCustomer(CustomerViewModel model)
    {
        if (_customerRepo.ExistsByEmail(model.Email))
        {
            _logService.CreateLogAsync(LogTags.Error, "Bu e-posta zaten kayıtlı");
            return new ServiceResult { Success = false, Message = $"Bu e-posta zaten kayıtlı: {model.Email}" };
        }

        var customer = new Customer
        {
            FirstName = model.FirstName,
            Email = model.Email,
            LastName = model.LastName,
            CreatedAt = DateTime.UtcNow,
            isDeleted = false,
            Region = model.Region,
        };

        _customerRepo.Add(customer);

        return new ServiceResult { Success = true };
    }

    public (List<Customer> Customers, int TotalCount) ListCustomers(string? search, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

        if (!string.IsNullOrWhiteSpace(search))
        {
            _logService.CreateLogAsync(LogTags.FilterAction, $"Müşteri filresi arandı: \"{search}\"");
        }

        var customers = _customerRepo.GetFilteredCustomers(search, skip, pageSize, out int totalCount);
        return (customers, totalCount);
    }


}
