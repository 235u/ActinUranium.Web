using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;

namespace ActinUranium.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IActionDescriptorCollectionProvider _provider;

        public HomeController(IActionDescriptorCollectionProvider provider)
        {
            _provider = provider;
        }

        public ViewResult Index()
        {
            return View();
        }

        // Razor Pages do not support localized view files, see: https://github.com/aspnet/AspNetCore/issues/4873
        // See also: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2#configure-localization
        public ViewResult Imprint()
        {
            return View();
        }

        [Route("/sitemap.xml")]
        public ContentResult Sitemap()
        {
            throw new NotImplementedException();
        }
    }
}
