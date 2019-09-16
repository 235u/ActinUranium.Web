using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ActinUranium.Web.Services;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System;

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

        public static IApplicationBuilder UseRewriter(
            this IApplicationBuilder app, Action<RewriteOptions> configureOptions)
        {
            var options = new RewriteOptions();
            configureOptions(options);
            return app.UseRewriter(options);
        }

        public static IApplicationBuilder UseStaticFiles(
            this IApplicationBuilder app, Action<StaticFileOptions> configureOptions)
        {
            var options = new StaticFileOptions();
            configureOptions(options);
            return app.UseStaticFiles(options);
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
