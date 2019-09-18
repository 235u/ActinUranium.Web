using ActinUranium.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ActinUranium.Web
{
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Not supported by the framework")]
    public sealed class Startup
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
            app.UseConfiguredRequestLocalization();
            app.UseConfiguredRewriter();
            app.UseConfiguredStaticFiles();
            app.UseConfiguredMvc();
            app.UseContentTypeOptions();
            app.UseContentSecurityPolicy();
            app.UseFrameOptions();
        }
    }
}
