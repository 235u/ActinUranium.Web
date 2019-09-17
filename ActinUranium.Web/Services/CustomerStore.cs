using ActinUranium.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActinUranium.Web.Services
{
    public sealed class CustomerStore
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerStore(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IOrderedQueryable<Customer> CustomersQuery => 
            _dbContext.Customers
                .Include(c => c.Logo)
                .OrderBy(c => c.Name);

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await CustomersQuery.ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync(int count)
        {
            return await CustomersQuery.Take(count).ToListAsync();
        }
    }
}
