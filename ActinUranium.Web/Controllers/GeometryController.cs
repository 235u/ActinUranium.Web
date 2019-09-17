using ActinUranium.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ActinUranium.Web.Controllers
{
    public class GeometryController : Controller
    {
        private readonly GeometryStore _service;

        public GeometryController(GeometryStore service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            IEnumerable<string> model = _service.GetSources();
            return View(model);
        }
    }
}
