using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ActinUranium.Web.Extensions;
using ActinUranium.Web.Services;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            services.AddRouting(ConfigureRouting);

            services.AddMvc(ConfigureMvc)
                .AddRazorPagesOptions(ConfigureRazorPages)
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(ConfigureDataAnnotationsLocalization)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHsts(ConfigureHsts);
            services.AddDbContext<ApplicationDbContext>(ConfigureDbContext);
            services.AddTransient<CreationStore>();
            services.AddTransient<CustomerStore>();
            services.AddTransient<GeometryService>();
            services.AddTransient<HeadlineStore>();
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

        private static void ConfigureRouting(RouteOptions options)
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
            options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
        }

        private static void ConfigureMvc(MvcOptions options)
        {
            var transformer = new SlugifyParameterTransformer();
            var convention = new RouteTokenTransformerConvention(transformer);
            options.Conventions.Add(convention);
        }

        private static void ConfigureRazorPages(RazorPagesOptions options)
        {
            var transformer = new SlugifyParameterTransformer();
            var convention = new PageRouteTransformerConvention(transformer);
            options.Conventions.Add(convention);
        }

        private static void ConfigureDataAnnotationsLocalization(MvcDataAnnotationsLocalizationOptions options)
        {
            options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(Resources));
        }

        private static void ConfigureHsts(HstsOptions options)
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(365);
        }

        private static void ConfigureDbContext(DbContextOptionsBuilder options)
        {
            const string DatabaseName = "ActinUranium.Web";
            options.UseInMemoryDatabase(DatabaseName);
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
