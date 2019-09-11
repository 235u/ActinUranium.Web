using Microsoft.EntityFrameworkCore;
using ActinUranium.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActinUranium.Web.Services
{
    public sealed class CreationStore
    {
        private readonly ApplicationDbContext _dbContext;

        public CreationStore(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IQueryable<Creation> CreationsQuery =>
            _dbContext.Creations
                .Include(c => c.CreationImages)
                    .ThenInclude(ci => ci.Image)
                .Include(c => c.Customer);

        private IOrderedQueryable<Creation> OrderedCreationsQuery =>
            CreationsQuery.OrderByDescending(c => c.ReleaseDate);

        public async Task<Creation> GetCreationAsync(string slug)
        {
            return await CreationsQuery.SingleAsync(c => c.Slug == slug);
        }

        // Signal that no laziness is involved, see: https://stackoverflow.com/a/32650559
        public async Task<IReadOnlyCollection<Creation>> GetCreationsAsync()
        {
            return await OrderedCreationsQuery.ToListAsync();
        }

        public async Task<IReadOnlyCollection<Creation>> GetCreationsAsync(int count)
        {
            return await OrderedCreationsQuery.Take(count)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Creation>> GetRelatedCreationsAsync(Creation creation, int count)
        {
            var prevCreations = await OrderedCreationsQuery.Where(c => c.ReleaseDate < creation.ReleaseDate)
                .Take(count)
                .ToListAsync();
            
            if (prevCreations.Count < count)
            {
                var remainingCount = count - prevCreations.Count;
                var nextCreations = await CreationsQuery.Where(c => c.ReleaseDate > creation.ReleaseDate)
                    .OrderBy(c => c.ReleaseDate)
                    .Take(remainingCount)
                    .OrderByDescending(c => c.ReleaseDate)
                    .ToListAsync();

                // For performance considerations, see: https://stackoverflow.com/q/15516462
                nextCreations.AddRange(prevCreations);
                return nextCreations;
            }
            else
            {
                return prevCreations;
            }
        }
    }
}
