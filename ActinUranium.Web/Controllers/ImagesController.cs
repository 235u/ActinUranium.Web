using ActinUranium.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ActinUranium.Web.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ImageStore _service;

        public ImagesController(ImageStore service)
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
