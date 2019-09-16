using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ActinUranium.Web.Extensions;
using ActinUranium.Web.Services;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Net.Http.Headers;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace ActinUranium.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization();
            services.AddConfiguredRouting();
            services.AddConfiguredMvc();
            services.AddConfiguredHsts();
            services.AddApplicationDbContext();
            services.AddDataStores();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDataSeeding();
            app.UseRequestLocalization(ConfigureRequestLocalization);
            app.UseRewriter(ConfigureRewriter);
            app.UseStaticFiles(ConfigureCacheControl);
            app.UseContentTypeOptions();
            app.UseContentSecurityPolicy();
            app.UseFrameOptions();
            app.UseMvc(ConfigureRoutes);
        }

        public static void ConfigureRequestLocalization(RequestLocalizationOptions options)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("de"),
                new CultureInfo("en")
            };

            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.DefaultRequestCulture = new RequestCulture("de");
        }

        private static void ConfigureRewriter(RewriteOptions options)
        {
            options.AddRedirectToHttpsPermanent();
            options.AddRedirectToWwwPermanent();
        }

        private void ConfigureCacheControl(StaticFileOptions options)
        {
            options.OnPrepareResponse = (context) =>
            {
                const int CachePeriodInSeconds = 31_536_000; // 1 year
                string cacheControlHeaderValue = $"public, max-age={CachePeriodInSeconds}";
                context.Context.Response.Headers.Append(HeaderNames.CacheControl, cacheControlHeaderValue);
            };
        }

        private static void ConfigureRoutes(IRouteBuilder routes)
        {
            routes.MapRoute(name: "default", template: "{controller:slugify=Home}/{action:slugify=Index}/{slug?}");
        }
    }
}
