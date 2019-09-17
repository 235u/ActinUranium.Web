using ActinUranium.Web.Models;
using ActinUranium.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActinUranium.Web.ViewComponents
{
    public class RelatedCreationSection : ViewComponent
    {
        private readonly CreationStore _store;

        public RelatedCreationSection(CreationStore store)
        {
            _store = store;
        }

        public async Task<IViewComponentResult> InvokeAsync(Creation creation, int count)
        {
            IReadOnlyCollection<Creation> model = await _store.GetRelatedCreationsAsync(creation, count);
            return View(model);
        }
    }
}
