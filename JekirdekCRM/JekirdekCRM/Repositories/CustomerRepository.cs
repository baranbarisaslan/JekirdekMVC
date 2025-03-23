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
    }
}
