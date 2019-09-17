using ActinUranium.Web.Models;
using ActinUranium.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ActinUranium.Web.Controllers
{
    [Display(Name = "Gaga")]
    public class HeadlinesController : Controller
    {
        private readonly HeadlineStore _store;

        public HeadlinesController(HeadlineStore store)
        {
            _store = store;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Headline> model = await _store.GetHeadlinesAsync();
            return View(model);
        }

        [Route("headlines/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            Headline model = await _store.GetHeadlineAsync(slug);
            return View(model);
        }
    }
}
