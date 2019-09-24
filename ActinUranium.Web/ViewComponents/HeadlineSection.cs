using ActinUranium.Web.Models;
using ActinUranium.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            IReadOnlyCollection<Headline> model = await _store.GetRepresentativeHeadlinesAsync(count);
            return View(model);
        }
    }
}
