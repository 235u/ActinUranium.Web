using ActinUranium.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace ActinUranium.Web
{
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Not supported by the framework")]
    public sealed class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization();
            services.AddConfiguredRouting();
            services.AddConfiguredMvc();
            services.AddConfiguredHsts();
            services.AddApplicationDbContext();
            services.AddApplicationDbInitializer();
            services.AddDataStores();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseHsts();
                app.UseContentTypeOptions();
                app.UseContentSecurityPolicy();
                app.UseFrameOptions();
                app.UseReferrerPolicy();
            }

            app.UseApplicationDbInitializer();
            app.UseConfiguredRequestLocalization();
            app.UseConfiguredRewriter();
            app.UseConfiguredStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller:slugify=Home}/{action:slugify=Index}/{slug?}");
            });
        }
    }
}
