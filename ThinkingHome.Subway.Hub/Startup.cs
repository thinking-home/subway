using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ThinkingHome.Alice.Service;
using ThinkingHome.DeviceModel.Remoting.ProxyServer;

namespace ThinkingHome.Subway.Hub
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddDeviceRemotingProxy();

            // TODO: реальный маппинг пользователя OAuth → hostId; пока одно домохозяйство
            services.AddSingleton<IHostIdResolver>(new StaticHostIdResolver("home"));

            services
                .AddControllers()
                .AddApplicationPart(typeof(AliceController).Assembly)
                .AddControllersAsServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/api-{Date}.txt");

            app.UseRouting().UseEndpoints(endpoints =>
            {
                endpoints.MapHub<DeviceHub>("/hub");
                endpoints.MapControllers();
            });
        }
    }
}
