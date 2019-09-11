using Microsoft.AspNetCore.Mvc;
using ActinUranium.Web.Services;
using System.Threading.Tasks;

namespace ActinUranium.Web.ViewComponents
{
    public class CustomerSection : ViewComponent
    {
        private readonly CustomerStore _store;

        public CustomerSection(CustomerStore store)
        {
            _store = store;
        }

        public async Task<IViewComponentResult> InvokeAsync(int customerCount)
        {
            var model = await _store.GetCustomersAsync(customerCount);
            return View(model);
        }
    }
}
