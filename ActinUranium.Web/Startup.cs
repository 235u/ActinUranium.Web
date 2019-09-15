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

namespace ActinUranium.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization();
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
                options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
            });            

            var transformer = new SlugifyParameterTransformer();
            services.AddMvc(options =>
                {
                    var convention = new RouteTokenTransformerConvention(transformer);
                    options.Conventions.Add(convention);
                })
                .AddRazorPagesOptions(options =>
                {
                    var convention = new PageRouteTransformerConvention(transformer);
                    options.Conventions.Add(convention);
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(Resources));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            const string DatabaseName = "ActinUranium.Web";
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(DatabaseName));

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
            app.ConfigureRequestLocalization();
            app.ConfigurePermanentRedirects();
            app.ConfigureStaticFileCaching();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller:slugify=Home}/{action:slugify=Index}/{slug?}");
            });
        }
    }
}
