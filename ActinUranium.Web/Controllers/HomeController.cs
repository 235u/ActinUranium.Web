using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ActinUranium.Web.Controllers
{
    public sealed class HomeController : Controller
    {
        public ViewResult Index() => View();

        // Razor Pages do not support localized view files, see: https://github.com/aspnet/AspNetCore/issues/4873
        // See also: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2#configure-localization
        public ViewResult Imprint() => View();
        
        [Route("/sitemap.xml")]
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Reviewed")]
        public ContentResult Sitemap() => throw new NotImplementedException();
    }
}
