using JekirdekCRM.Models.DBModels;
using JekirdekCRM.Models.ViewModels;

namespace JekirdekCRM.Repositories
{
    public interface ICustomerRepository
    {
        void Add(Customer customer);
        bool ExistsByEmail(string email);

        List<Customer> GetFilteredCustomers(string? search, int skip, int take, out int totalCount);

        void SoftDelete(int id);

        void Update(Customer customer);

        Customer? GetById(int id);

        int GetTotalCustomerCount();

        int GetTodayCustomerCount();

        int GetThisMonthCustomerCount();

        List<RegionCount> GetCustomerCountByRegion();

        List<MonthlyCustomerCount> GetMonthlyCustomerCount(int months = 6);
    }

}
