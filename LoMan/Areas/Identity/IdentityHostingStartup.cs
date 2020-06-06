using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(LoMan.Areas.Identity.IdentityHostingStartup))]
namespace LoMan.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}