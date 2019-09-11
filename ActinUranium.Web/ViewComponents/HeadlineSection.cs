using Microsoft.AspNetCore.Mvc;
using ActinUranium.Web.Services;
using System.Threading.Tasks;

namespace ActinUranium.Web.ViewComponents
{
    public class HeadlineSection : ViewComponent
    {
        private readonly HeadlineStore _store;

        public HeadlineSection(HeadlineStore store)
        {
            _store = store;
        }

        public async Task<IViewComponentResult> InvokeAsync(int headlineCount)
        {
            var model = await _store.GetRepresentativeHeadlinesAsync(headlineCount);
            return View(model);
        }
    }
}
