using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace ThinkingHome.DeviceModel.Remoting.ProxyServer;

public static class ProxyServerServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует реестр удалённых хостов (singleton). Сам SignalR (<c>AddSignalR</c>) и маппинг
    /// хаба (<c>app.MapHub&lt;DeviceHub&gt;("/hub")</c>) настраивает приложение-прокси.
    /// </summary>
    public static IServiceCollection AddDeviceRemotingProxy(this IServiceCollection services)
    {
        services.AddSingleton<RemoteHostRegistry>();
        services.AddSingleton<IRemoteHostRegistry>(sp => sp.GetRequiredService<RemoteHostRegistry>());
        return services;
    }

    /// <summary>
    /// JWT-аутентификация соединений хостов: токен коннектора (aud=connector), подписанный
    /// <paramref name="signingKey"/>. Токен SignalR берётся из query (<c>access_token</c>) для <c>/hub</c>.
    /// </summary>
    public static IServiceCollection AddDeviceRemotingProxyAuth(this IServiceCollection services, string signingKey)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = HostToken.ConnectorValidation(signingKey);
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // SignalR (WebSocket) передаёт токен в query, а не в заголовке
                        var token = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(token) &&
                            context.HttpContext.Request.Path.StartsWithSegments(DeviceHub.Path))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    },
                };
            });

        services.AddAuthorization();
        return services;
    }
}
