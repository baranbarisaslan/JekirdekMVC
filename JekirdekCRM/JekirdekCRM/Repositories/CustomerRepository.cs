using JekirdekCRM.Models;
using JekirdekCRM.Models.DBModels;

namespace JekirdekCRM.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseContext _context;

        public CustomerRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public bool ExistsByEmail(string email)
        {
            return _context.Customers.Any(c => c.Email == email);
        }

        public List<Customer> GetFilteredCustomers(string? search, int skip, int take, out int totalCount)
        {
            var query = _context.Customers.Where(c => !c.isDeleted).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    c.FirstName.Contains(search) ||
                    c.LastName.Contains(search) ||
                    c.Email.Contains(search) ||
                    c.Region.Contains(search));
            }

            totalCount = query.Count();

            return query
                .OrderBy(c => c.FirstName)
                .Skip(skip)
                .Take(take)
                .ToList();
        }


    }
}
