using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ActinUranium.Web.Services;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ActinUranium.Web.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDataSeeding(this IApplicationBuilder app)
        {
            var serviceType = typeof(IServiceScopeFactory);
            var serviceScopeFactory = app.ApplicationServices.GetService(serviceType) as IServiceScopeFactory;

            using (IServiceScope serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Seed();
            }

            return app;
        }

        public static IApplicationBuilder UseConfiguredRequestLocalization(this IApplicationBuilder app)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("de"),
                new CultureInfo("en")
            };

            app.UseRequestLocalization(options =>
            {
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.DefaultRequestCulture = new RequestCulture("de");
            });

            return app;
        }

        public static IApplicationBuilder UseConfiguredRewriter(this IApplicationBuilder app)
        {
            var options = new RewriteOptions();

            options.AddRedirectToHttpsPermanent();
            options.AddRedirectToWwwPermanent();

            return app.UseRewriter(options);
        }

        public static IApplicationBuilder UseConfiguredStaticFiles(this IApplicationBuilder app)
        {
            var options = new StaticFileOptions()
            {
                OnPrepareResponse = (context) =>
                {
                    const int CachePeriodInSeconds = 31_536_000; // 1 year
                    string cacheControlHeaderValue = $"public, max-age={CachePeriodInSeconds}";
                    context.Context.Response.Headers.Append(HeaderNames.CacheControl, cacheControlHeaderValue);
                }
            };

            return app.UseStaticFiles(options);
        }

        public static IApplicationBuilder UseConfiguredMvc(this IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller:slugify=Home}/{action:slugify=Index}/{slug?}");
            });

            return app;
        }

        /// <summary>
        /// Sets the <c>X-Content-Type-Options</c> response header to prevent XSS attacks.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks><c>X-Content-Type-Options</c> is a non-standard HTTP header telling browsers not to load
        /// scripts and stylesheets unless the server indicates the correct MIME type. Without this header, the browsers
        /// can incorrectly detect files as scripts and stylesheets, leading to XSS attacks. As such, all sites must set
        /// the <c>X-Content-Type-Options</c> header and the appropriate MIME types for files that they serve.</remarks>
        /// <seealso href="https://infosec.mozilla.org/guidelines/web_security#x-content-type-options">Mozilla's Web
        /// Security Guidelines, X-Content-Type-Options</seealso>
        public static IApplicationBuilder UseContentTypeOptions(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                await next();
            });

            return app;
        }

        /// <summary>
        /// Sets the Content Security Policy's <c>frame-ancestors</c> directive to prevent clickjacking attacks.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>The <c>frame-ancestors</c> directive, which is not yet supported by all major browsers, supercedes
        /// the <c>X-Frame-Options</c> header.</remarks>
        /// <seealso cref="UseFrameOptions(IApplicationBuilder)"/>
        /// <seealso cref="https://infosec.mozilla.org/guidelines/web_security.html#content-security-policy">Mozilla's
        /// Web Security Guidelines, Content Security Policy</seealso>
        public static IApplicationBuilder UseContentSecurityPolicy(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add(HeaderNames.ContentSecurityPolicy, "frame-ancestors 'none'");
                await next();
            });

            return app;
        }

        /// <summary>
        /// Sets the <c>X-Frame-Options</c> response header to prevent clickjacking attacks.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks><c>X-Frame-Options</c> is a HTTP header controlling site's framing within an iframe.</remarks>
        /// <seealso href="https://infosec.mozilla.org/guidelines/web_security#x-frame-options">Mozilla's Web Security
        /// Guidelines, X-Frame-Options</seealso>
        public static IApplicationBuilder UseFrameOptions(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                await next();
            });

            return app;
        } 
    }
}
