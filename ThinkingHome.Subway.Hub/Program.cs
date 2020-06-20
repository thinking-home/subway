using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ThinkingHome.Subway.Hub
{
    class Program
    {
        static IHost CreateHost(string[] args)
        {
            string certPath = "/home/dima117a/merged.pfx";
            string certPassword = "changeit";

            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .ConfigureKestrel(cfg =>
                        cfg.Listen(IPAddress.Any, 443, opt => opt.UseHttps(certPath, certPassword)))
                    .UseStartup<Startup>());

            return hostBuilder.Build();
        }

        static void Main(string[] args)
        {
            using (var host = CreateHost(args))
            {
                // host.Start();
                host.Run();
            }

            // var hubContext = host.Services.GetService<IHubContext<TestHub>>();
        }
    }
}
