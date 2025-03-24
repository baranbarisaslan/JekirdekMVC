using JekirdekCRM.Models;
using JekirdekCRM.Models.DBModels;
using JekirdekCRM.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

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
            return _context.Customers.Where(c => !c.isDeleted).Any(c => c.Email == email);
        }

        public List<Customer> GetFilteredCustomers(string? search, int skip, int take, out int totalCount)
        {
            var query = _context.Customers.Where(c => !c.isDeleted).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = $"%{search}%";
                query = query.Where(c =>
                    EF.Functions.ILike(c.FirstName, search) ||
                    EF.Functions.ILike(c.LastName, search) ||
                    EF.Functions.ILike(c.Email, search) ||
                    EF.Functions.ILike(c.Region, search));
            }

            totalCount = query.Count();

            return query
                .Skip(skip)
                .Take(take)
                .ToList();
        }


        public void SoftDelete(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == id && !c.isDeleted);
            if (customer != null)
            {
                customer.isDeleted = true;
                _context.SaveChanges();
            }
        }

        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        public Customer? GetById(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.Id == id && !c.isDeleted);
        }


        public int GetTotalCustomerCount()
        {
            return _context.Customers.Count(c => !c.isDeleted);
        }

        public int GetTodayCustomerCount()
        {
            var today = DateTime.UtcNow.Date;
            return _context.Customers.Count(c => !c.isDeleted && c.CreatedAt.Date == today);
        }

        public int GetThisMonthCustomerCount()
        {
            var now = DateTime.UtcNow;
            return _context.Customers.Count(c =>
                !c.isDeleted &&
                c.CreatedAt.Month == now.Month &&
                c.CreatedAt.Year == now.Year);
        }


        public List<RegionCount> GetCustomerCountByRegion()
        {
            return _context.Customers
                .Where(c => !c.isDeleted)
                .GroupBy(c => c.Region)
                .Select(g => new RegionCount
                {
                    Region = g.Key,
                    Count = g.Count()
                }).ToList();
        }

        public List<MonthlyCustomerCount> GetMonthlyCustomerCount(int months = 6)
        {
            var cutoff = DateTime.UtcNow.AddMonths(-months);

            return _context.Customers
                .Where(c => !c.isDeleted && c.CreatedAt >= cutoff)
                .AsEnumerable() // 🔑 burası kritik
                .GroupBy(c => c.CreatedAt.ToString("yyyy-MM"))
                .Select(g => new MonthlyCustomerCount
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Month)
                .ToList();
        }



    }
}
