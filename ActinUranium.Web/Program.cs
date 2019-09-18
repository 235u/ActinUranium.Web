using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ActinUranium.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IWebHostBuilder webHost = CreateWebHostBuilder(args);
            webHost.Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}
