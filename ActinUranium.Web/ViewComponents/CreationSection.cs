using Microsoft.AspNetCore.Mvc;
using ActinUranium.Web.Services;
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

        public async Task<IViewComponentResult> InvokeAsync(int creationCount)
        {
            var model = await _store.GetCreationsAsync(creationCount);
            return View(model);
        }
    }
}
