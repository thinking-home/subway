using Microsoft.AspNetCore.SignalR;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Remoting.ProxyServer;

/// <summary>
/// SignalR-хаб на прокси. Домашние коннекторы подключаются сюда (JWT с hostId в claims).
/// На connect/disconnect обновляет реестр; принимает Report'ы (дом → прокси).
/// Команды идут в обратную сторону (server → client) из <see cref="RemoteHost"/>.
/// </summary>
public sealed class DeviceHub(RemoteHostRegistry registry) : Hub
{
    public const string HostIdClaim = "hostId";

    public override Task OnConnectedAsync()
    {
        var hostId = Context.User?.FindFirst(HostIdClaim)?.Value;
        if (string.IsNullOrEmpty(hostId))
        {
            Context.Abort();
            return Task.CompletedTask;
        }

        registry.Attach(hostId, Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        registry.Detach(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    /// <summary>Отчёт об изменении состояния (дом → прокси).</summary>
    public void Report(StateChange change) => registry.DispatchReport(Context.ConnectionId, change);
}
