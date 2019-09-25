using ActinUranium.Web.Models;
using ActinUranium.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActinUranium.Web.ViewComponents
{
    public class ReleaseSection : ViewComponent
    {
        private readonly ReleaseStore _releaseStore;

        public ReleaseSection(ReleaseStore releaseStore)
        {
            _releaseStore = releaseStore;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            IReadOnlyCollection<IRelease> model = await _releaseStore.GetLatestReleasesAsync(count);
            return View(model);
        }
    }
}
