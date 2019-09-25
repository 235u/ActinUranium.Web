using ActinUranium.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActinUranium.Web.Services
{
    public sealed class ReleaseStore
    {
        private readonly CreationStore _creationStore;
        private readonly HeadlineStore _headlineStore;

        public ReleaseStore(CreationStore creationStore, HeadlineStore headlineStore)
        {
            _creationStore = creationStore;
            _headlineStore = headlineStore;
        }

        public async Task<IReadOnlyCollection<IRelease>> GetLatestReleasesAsync(int count)
        {
            // Take twice the required release count to ensure the chronological release order.
            IReadOnlyCollection<IRelease> creations = await _creationStore.GetCreationsAsync(count);
            IReadOnlyCollection<IRelease> headlines = await _headlineStore.GetRepresentativeHeadlinesAsync(count);

            return creations.Concat(headlines)
                .OrderByDescending(r => r.ReleaseDate)
                .Take(count)
                .ToList();
        }
    }
}
