using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace ThinkingHome.DeviceModel.Remoting.ProxyServer;

public static class ProxyServerServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует реестр удалённых хостов (singleton). Сам SignalR (<c>AddSignalR</c>) и маппинг
    /// хаба (<c>app.MapDeviceHub()</c>) настраивает приложение-прокси.
    /// </summary>
    public static IServiceCollection AddDeviceRemotingProxy(this IServiceCollection services)
    {
        services.AddSingleton<RemoteHostRegistry>();
        services.AddSingleton<IRemoteHostRegistry>(sp => sp.GetRequiredService<RemoteHostRegistry>());
        return services;
    }

    /// <summary>
    /// JWT-схемы прокси (один ключ подписи): <see cref="HostToken.ConnectorScheme"/> (aud=connector,
    /// токен из query для <c>/hub</c>, по умолчанию) и <see cref="HostToken.AliceScheme"/> (aud=alice,
    /// для запросов Алисы). Валидация только по подписи — прокси остаётся stateless.
    /// </summary>
    public static IServiceCollection AddDeviceRemotingProxyAuth(this IServiceCollection services, string signingKey)
    {
        services
            .AddAuthentication(HostToken.ConnectorScheme)
            .AddJwtBearer(HostToken.ConnectorScheme, options =>
            {
                options.TokenValidationParameters = HostToken.ConnectorValidation(signingKey);
                options.Events = new JwtBearerEvents { OnMessageReceived = ReadHubTokenFromQuery };
            })
            .AddJwtBearer(HostToken.AliceScheme, options =>
            {
                options.TokenValidationParameters = HostToken.AliceValidation(signingKey);
            });

        services.AddAuthorization();
        return services;
    }

    private static Task ReadHubTokenFromQuery(MessageReceivedContext context)
    {
        // SignalR (WebSocket) передаёт токен в query, а не в заголовке
        var token = context.Request.Query["access_token"];
        if (!string.IsNullOrEmpty(token) && context.HttpContext.Request.Path.StartsWithSegments(DeviceHub.Path))
        {
            context.Token = token;
        }

        return Task.CompletedTask;
    }
}
