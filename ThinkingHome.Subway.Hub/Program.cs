using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ThinkingHome.Subway.Hub
{
    class Program
    {
        static void Main(string[] args)
        {
            // string certPath = "/home/dima117a/merged.pfx";
            // string certPassword = "changeit";

            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    // .ConfigureKestrel(cfg =>
                    //     cfg.Listen(IPAddress.Any, 443, opt => opt.UseHttps(certPath, certPassword)))
                    .UseStartup<Startup>());

            using var host = hostBuilder.Build();
            // host.Start();
            host.Run();
        }
    }
}