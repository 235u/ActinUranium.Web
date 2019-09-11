using Microsoft.AspNetCore.Mvc;
using ActinUranium.Web.Models;
using ActinUranium.Web.Services;
using System.Threading.Tasks;

namespace ActinUranium.Web.ViewComponents
{
    public class RelatedHeadlineSection : ViewComponent
    {
        private readonly HeadlineStore _store;

        public RelatedHeadlineSection(HeadlineStore store)
        {
            _store = store;
        }

        public async Task<IViewComponentResult> InvokeAsync(Headline headline)
        {
            var model = await _store.GetRelatedHeadlinesAsync(headline);
            return View(model);
        }
    }
}
