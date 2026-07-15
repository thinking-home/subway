using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Remoting.ProxyServer;

/// <summary>
/// SignalR-хаб на прокси. Домашние коннекторы подключаются сюда с JWT (aud=connector, hostId в claim).
/// На connect/disconnect обновляет реестр; принимает Report'ы (дом → прокси). Команды идут в обратную
/// сторону (server → client) из <see cref="RemoteHost"/>.
/// </summary>
[Authorize]
public sealed class DeviceHub(RemoteHostRegistry registry) : Hub
{
    /// <summary>Канонический путь хаба — единый источник и для маппинга, и для извлечения токена из query.</summary>
    public const string Path = "/hub";

    public override Task OnConnectedAsync()
    {
        // hostId — из валидированного JWT; соединение без него отклоняем
        var hostId = Context.User?.FindFirst(HostToken.HostIdClaim)?.Value;
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
