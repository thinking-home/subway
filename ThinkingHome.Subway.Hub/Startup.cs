using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ThinkingHome.Alice.Service;
using ThinkingHome.Alice.Service.Stub;

namespace ThinkingHome.Subway.Hub
{
    public class Startup(IConfiguration configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var bulbs = new IDevice[]
            {
                new TestBulb("1"),
                new TestBulb("12"),
            }.ToDictionary(b => b.Id);

            services.AddSingleton(bulbs);

            services
                .AddControllers()
                .AddApplicationPart(typeof(AliceController).Assembly)
                .AddControllersAsServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/api-{Date}.txt");

            app.UseRouting().UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}