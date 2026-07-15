using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ThinkingHome.DeviceModel.Remoting.ProxyServer;

namespace ThinkingHome.Subway.Hub
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 1 && args[0] == "issue-host-token")
            {
                IssueHostToken(args);
                return;
            }

            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .Build()
                .Run();
        }

        // CLI: dotnet run --project ThinkingHome.Subway.Hub -- issue-host-token --hostId <id>
        static void IssueHostToken(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var signingKey = config["Jwt:SigningKey"]
                ?? throw new InvalidOperationException("Jwt:SigningKey не задан (appsettings/env).");

            var hostId = GetOption(args, "--hostId")
                ?? throw new ArgumentException("Укажите --hostId <id>.");

            Console.WriteLine(HostToken.IssueConnectorToken(signingKey, hostId));
        }

        static string GetOption(string[] args, string name)
        {
            var i = Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : null;
        }
    }
}
