using ActinUranium.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ActinUranium.Web
{
    public sealed partial class Startup
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
