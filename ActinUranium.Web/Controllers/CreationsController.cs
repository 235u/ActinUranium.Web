using Microsoft.AspNetCore.Mvc;
using ActinUranium.Web.Models;
using ActinUranium.Web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActinUranium.Web.Controllers
{
    public sealed class CreationsController : Controller
    {
        private readonly CreationStore _store;

        public CreationsController(CreationStore store)
        {
            _store = store;
        }

        public async Task<IActionResult> Index()
        {
            IReadOnlyCollection<Creation> model = await _store.GetCreationsAsync();
            return View(model);
        }

        [Route("creations/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            Creation model = await _store.GetCreationAsync(slug);
            return View(model);
        }
    }
}
