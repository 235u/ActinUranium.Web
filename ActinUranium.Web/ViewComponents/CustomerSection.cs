using ActinUranium.Web.Models;
using ActinUranium.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            IEnumerable<Customer> model = await _store.GetCustomersAsync(count);
            return View(model);
        }
    }
}
