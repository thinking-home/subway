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
}
