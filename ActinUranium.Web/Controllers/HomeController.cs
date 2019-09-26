using Microsoft.AspNetCore.Mvc;
using System;

namespace ActinUranium.Web.Controllers
{
    public sealed class HomeController : Controller
    {
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
