using Microsoft.AspNetCore.Mvc;
using ActinUranium.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActinUranium.Web.Controllers
{
    public class GeometryController : Controller
    {
        private readonly GeometryService _service;

        public GeometryController(GeometryService service)
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
