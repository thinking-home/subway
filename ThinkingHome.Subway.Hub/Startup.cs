using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ThinkingHome.Alice.Service;

namespace ThinkingHome.Subway.Hub
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddControllers().AddNewtonsoftJson(opts =>
                {
                    opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    opts.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .AddApplicationPart(typeof(AliceController).Assembly)
                .AddControllersAsServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/api-{Date}.txt");

            app.UseRouting().UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TestHub>("/hub");
                endpoints.MapControllers();
            });
        }
    }
}
