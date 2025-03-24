using JekirdekCRM.Models.DBModels;

namespace JekirdekCRM.Repositories
{
    public interface ICustomerRepository
    {
        void Add(Customer customer);
        bool ExistsByEmail(string email);

        List<Customer> GetFilteredCustomers(string? search, int skip, int take, out int totalCount);
    }

}
