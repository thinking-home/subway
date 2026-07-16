using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ThinkingHome.Alice.Service;
using ThinkingHome.DeviceModel.Remoting.ProxyServer;

namespace ThinkingHome.Subway.Hub
{
    public class Startup(IConfiguration configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddDeviceRemotingProxy();
            services.AddDeviceRemotingProxyAuth(SigningKey());

            services.AddSingleton<IHostIdResolver, ClaimHostIdResolver>();

            services
                .AddControllers()
                .AddApplicationPart(typeof(AliceController).Assembly)
                .AddControllersAsServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/api-{Date}.txt");

            app.UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDeviceHub();
                    endpoints.MapControllers();
                });
        }

        private string SigningKey() =>
            configuration["Jwt:SigningKey"]
            ?? throw new System.InvalidOperationException("Jwt:SigningKey не задан (appsettings/env).");
    }
}
