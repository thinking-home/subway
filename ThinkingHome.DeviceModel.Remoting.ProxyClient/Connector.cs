using Microsoft.AspNetCore.SignalR.Client;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Remoting.ProxyClient;

/// <summary>
/// Домашняя сторона ремоутинга: SignalR-клиент, подключающийся к прокси и оборачивающий локальный
/// <see cref="IDeviceHost"/>. Обрабатывает вызовы прокси (GetDevices/Query/Execute → локальный хост)
/// и пушит изменения состояния (Changed → Report). Идентичность (hostId) — в JWT из
/// <paramref name="accessTokenProvider"/>; прокси читает её из claims.
/// </summary>
public sealed class Connector : IAsyncDisposable
{
    private readonly IDeviceHost host;
    private readonly HubConnection connection;

    public Connector(IDeviceHost host, string url, Func<Task<string?>>? accessTokenProvider = null)
    {
        this.host = host;

        connection = new HubConnectionBuilder()
            .WithUrl(url, options =>
            {
                if (accessTokenProvider is not null)
                {
                    options.AccessTokenProvider = accessTokenProvider;
                }
            })
            .WithAutomaticReconnect()
            .Build();

        // прокси → дом (server → client с результатом)
        connection.On(DeviceHubMethods.GetDevices,
            () => host.GetDevicesAsync());
        connection.On<string, DeviceSnapshot>(DeviceHubMethods.Query,
            deviceId => host.QueryAsync(deviceId));
        connection.On<string, DeviceCommand, CommandOutcome>(DeviceHubMethods.Execute,
            (deviceId, command) => host.ExecuteAsync(deviceId, command));
    }

    /// <summary>Текущее состояние соединения с прокси.</summary>
    public HubConnectionState State => connection.State;

    public async Task StartAsync(CancellationToken ct = default)
    {
        host.Changed += OnHostChanged;
        await connection.StartAsync(ct);
    }

    public async Task StopAsync(CancellationToken ct = default)
    {
        host.Changed -= OnHostChanged;
        await connection.StopAsync(ct);
    }

    // дом → прокси: отчёт об изменении, best-effort (при разрыве теряется — Алиса перезапросит)
    private void OnHostChanged(StateChange change)
    {
        if (connection.State != HubConnectionState.Connected) return;
        _ = SendReportAsync(change);
    }

    private async Task SendReportAsync(StateChange change)
    {
        try
        {
            await connection.SendAsync(DeviceHubMethods.Report, change);
        }
        catch
        {
            // репорт best-effort; проглатываем ошибку отправки при разрыве
        }
    }

    public async ValueTask DisposeAsync()
    {
        host.Changed -= OnHostChanged;
        await connection.DisposeAsync();
    }
}
