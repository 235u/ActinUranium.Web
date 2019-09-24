using ActinUranium.Web.Models;
using ActinUranium.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActinUranium.Web.ViewComponents
{
    public class CreationSection : ViewComponent
    {
        private readonly CreationStore _store;

        public CreationSection(CreationStore store)
        {
            _store = store;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            IReadOnlyCollection<Creation> model = await _store.GetCreationsAsync(count);
            return View(model);
        }
    }
}
