using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using ActinUranium.Web.Services;
using System.Globalization;
using Microsoft.AspNetCore.Rewrite;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ActinUranium.Web.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        public static void UseDataSeeding(this IApplicationBuilder app)
        {
            var serviceScopeFactory = 
                app.ApplicationServices.GetService(typeof(IServiceScopeFactory)) as IServiceScopeFactory;
            using (IServiceScope serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Seed();
            }
        }

        public static void ConfigureRequestLocalization(this IApplicationBuilder app)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("de"),
                new CultureInfo("en")
            };

            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("de"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            app.UseRequestLocalization(options);
        }

        public static void ConfigurePermanentRedirects(this IApplicationBuilder app)
        {
            var options = new RewriteOptions();
            options.AddRedirectToHttpsPermanent();
            options.AddRedirectToWwwPermanent();
            app.UseRewriter(options);
        }

        public static void ConfigureStaticFileCaching(this IApplicationBuilder app)
        {
            var options = new StaticFileOptions()
            {
                OnPrepareResponse = (ctx) =>
                {                    
                    const int CachePeriodInSeconds = 31_536_000;
                    var cacheControlHeaderValue = $"public, max-age={CachePeriodInSeconds}";
                    ctx.Context.Response.Headers.Append("Cache-Control", cacheControlHeaderValue);
                }
            };

            app.UseStaticFiles(options);
        }
    }
}
