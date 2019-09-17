using ActinUranium.Web.Models;
using ActinUranium.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActinUranium.Web.Controllers
{
    public sealed class CustomersController : Controller
    {
        private readonly CustomerStore _store;

        public CustomersController(CustomerStore store)
        {
            _store = store;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Customer> model = await _store.GetCustomersAsync();
            return View(model);
        }
    }
}
