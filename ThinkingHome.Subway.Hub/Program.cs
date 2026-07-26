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
                // канон — env с префиксом THINKINGHOME_ (поверх стандартных источников,
                // непрефиксные продолжают работать); командная строка — последней
                .ConfigureAppConfiguration(config => config
                    .AddEnvironmentVariables("THINKINGHOME_")
                    .AddCommandLine(args))
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .Build()
                .Run();
        }

        // CLI: dotnet run --project ThinkingHome.Subway.Hub -- issue-host-token --hostId <id>
        static void IssueHostToken(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("THINKINGHOME_ENVIRONMENT");

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true);

            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                // окружение задано явно → его файл обязан существовать (защита от опечатки в имени)
                configBuilder.AddJsonFile($"appsettings.{environmentName}.json", optional: false);
            }

            var config = configBuilder
                .AddUserSecrets(typeof(Program).Assembly, optional: true)
                .AddEnvironmentVariables()
                .AddEnvironmentVariables("THINKINGHOME_")
                .Build();

            var signingKey = config["Jwt:SigningKey"]
                ?? throw new InvalidOperationException(
                    "Jwt:SigningKey не задан (user-secrets / env THINKINGHOME_Jwt__SigningKey).");

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
