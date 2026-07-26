using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace ThinkingHome.DeviceModel.Remoting.ProxyServer;

/// <summary>Маппинг SignalR-хаба устройств в приложении-прокси.</summary>
public static class ProxyServerEndpointExtensions
{
    /// <summary>Маппит <see cref="DeviceHub"/> на его канонический путь (<see cref="DeviceHub.Path"/>).</summary>
    public static IEndpointConventionBuilder MapDeviceHub(this IEndpointRouteBuilder endpoints)
        => endpoints.MapHub<DeviceHub>(DeviceHub.Path);
}
