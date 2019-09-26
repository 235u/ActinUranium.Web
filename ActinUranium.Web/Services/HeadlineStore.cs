using ActinUranium.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActinUranium.Web.Services
{
    public class HeadlineStore
    {
        private readonly ApplicationDbContext _dbContext;

        private IOrderedQueryable<Headline> HeadlinesQuery =>
            _dbContext.Headlines
                .Include(h => h.HeadlineImages)
                    .ThenInclude(hi => hi.Image)
                .Include(h => h.Tag)
                .Include(h => h.Author)
                .OrderByDescending(h => h.ReleaseDate)
                    .ThenBy(h => h.Title);

        private IQueryable<Headline> RepresentativeHeadlinesQuery => 
            HeadlinesQuery.Where(h => h.HeadlineImages.Any());

        public HeadlineStore(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Headline> GetHeadlineAsync(string slug)
        {
            return await _dbContext.Headlines
                .Include(h => h.Tag)
                .Include(h => h.HeadlineImages)
                    .ThenInclude(hi => hi.Image)
                .Include(h => h.Author)
                .SingleAsync(h => h.Slug == slug);
        }

        public async Task<IReadOnlyCollection<Headline>> GetHeadlinesAsync()
        {
            return await HeadlinesQuery.ToListAsync();
        }

        public async Task<IReadOnlyCollection<Headline>> GetHeadlinesAsync(string tagSlug)
        {
            return await HeadlinesQuery.Where(h => h.TagSlug == tagSlug).ToListAsync();
        }

        public async Task<IReadOnlyCollection<Headline>> GetRepresentativeHeadlinesAsync(int count)
        {
            return await RepresentativeHeadlinesQuery.Take(count).ToListAsync();
        }

        public async Task<IReadOnlyCollection<Headline>> GetRepresentativeHeadlinesAsync(Headline reference, int count)
        {
            return await RepresentativeHeadlinesQuery
                .Where(h => (h.Slug != reference.Slug) && (h.TagSlug == reference.TagSlug))
                .Take(count)
                .ToListAsync();
        }
    }
}
