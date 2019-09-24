using ActinUranium.Web.Models;
using ActinUranium.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActinUranium.Web.ViewComponents
{
    public class ContemporaryCreationSection : ViewComponent
    {
        private readonly CreationStore _store;

        public ContemporaryCreationSection(CreationStore store)
        {
            _store = store;
        }

        public async Task<IViewComponentResult> InvokeAsync(Creation reference, int count)
        {
            IReadOnlyCollection<Creation> model = await _store.GetContemporaryCreationsAsync(reference, count);
            return View(model);
        }
    }
}
