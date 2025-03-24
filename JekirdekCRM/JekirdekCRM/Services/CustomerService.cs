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
        _logService.CreateLogAsync(LogTags.FilterAction, $"Müşteri oluşturuldu: \"{customer.Email}\"");
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

    public ServiceResult DeleteCustomer(int id)
    {
        try
        {
            _customerRepo.SoftDelete(id);
            _logService.CreateLogAsync(LogTags.DatabaseAction, $"Müşteri silindi: {id}");
            return new ServiceResult { Success = true };
        }
        catch (Exception ex)
        {
            _logService.CreateLogAsync(LogTags.Exception, $"DeleteCustomer Exception → {ex.Message}");
            return new ServiceResult { Success = false, Message = "Silme işlemi başarısız." };
        }
    }


    public CustomerViewModel? GetCustomer(int id)
    {
        var customer = _customerRepo.GetById(id);
        if (customer == null)
            return null;

        return new CustomerViewModel
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            Region = customer.Region
        };
    }

    public ServiceResult EditCustomer(int id, CustomerViewModel model)
    {
        var customer = _customerRepo.GetById(id);
        if (customer == null)
            return new ServiceResult { Success = false, Message = "Müşteri bulunamadı." };

        customer.FirstName = model.FirstName;
        customer.LastName = model.LastName;
        customer.Email = model.Email;
        customer.Region = model.Region;

        _customerRepo.Update(customer);
        _logService.CreateLogAsync(LogTags.DatabaseAction, $"Müşteri güncellendi: {customer.Email}");

        return new ServiceResult { Success = true };
    }

    public IndexViewModel GetCustomerDashboardData()
    {
        return new IndexViewModel
        {
            TotalCustomerCount = _customerRepo.GetTotalCustomerCount(),
            TodayCustomerCount = _customerRepo.GetTodayCustomerCount(),
            ThisMonthCustomerCount = _customerRepo.GetThisMonthCustomerCount(),
            CustomersByRegion = _customerRepo.GetCustomerCountByRegion(),
            CustomersByMonth = _customerRepo.GetMonthlyCustomerCount()
        };
    }

}
