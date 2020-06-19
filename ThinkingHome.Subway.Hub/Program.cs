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
        static void Main(string[] args)
        {
            string certPath = "/home/dima117a/merged.pfx";
            string certPassword = "changeit";

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .ConfigureKestrel(cfg =>
                        cfg.Listen(IPAddress.Loopback, 443, opt => opt.UseHttps(certPath, certPassword)))
                    .UseStartup<Startup>())
                .Build();

            var hubContext = host.Services.GetService<IHubContext<TestHub>>();

            host.Start();

            Console.WriteLine($"SEND1231231241241241");

            var x = Console.ReadLine();

            while (x != "exit")
            {
                Console.WriteLine($"SEND: {x}");
                hubContext.Clients.All.SendAsync(TestHub.CLIENT_METHOD_NAME, x);
                x = Console.ReadLine();
            }

            host.Dispose();
        }
    }
}
