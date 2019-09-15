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

        public static IApplicationBuilder UseContentTypeOptions(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                await next();
            });

            return app;
        }

        public static IApplicationBuilder UseContentSecurityPolicy(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add(HeaderNames.ContentSecurityPolicy, "frame-ancestors 'none'");
                await next();
            });

            return app;
        }

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
