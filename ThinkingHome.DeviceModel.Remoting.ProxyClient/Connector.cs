using Microsoft.AspNetCore.SignalR.Client;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Remoting.ProxyClient;

/// <summary>
/// Домашняя сторона ремоутинга: SignalR-клиент, подключающийся к прокси и оборачивающий локальный
/// <see cref="IDeviceHost"/>. Обрабатывает вызовы прокси (GetDevices/Query/Execute → локальный хост),
/// пушит изменения (Changed → Report) и генерирует/проверяет OTP привязки (состояние OTP живёт здесь,
/// на хосте — прокси остаётся stateless). Идентичность — в JWT из <paramref name="accessTokenProvider"/>;
/// доставку OTP даёт потребитель через <see cref="IOtpDelivery"/>.
/// </summary>
public sealed class Connector : IAsyncDisposable
{
    private readonly IDeviceHost host;
    private readonly HubConnection connection;
    private readonly OtpState otp = new();

    public Connector(IDeviceHost host, IOtpDelivery otpDelivery, string url, Func<Task<string?>>? accessTokenProvider = null)
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
        connection.On(DeviceHubMethods.GetDevices, () => host.GetDevicesAsync());
        connection.On<string, DeviceSnapshot>(DeviceHubMethods.Query, deviceId => host.QueryAsync(deviceId));
        connection.On<string, DeviceCommand, CommandOutcome>(DeviceHubMethods.Execute,
            (deviceId, command) => host.ExecuteAsync(deviceId, command));

        // OTP привязки: состояние — на хосте, доставку даёт потребитель
        connection.On(DeviceHubMethods.GenerateLinkingOtp, () => otp.GenerateAndDeliverAsync(otpDelivery));
        connection.On<string, bool>(DeviceHubMethods.ValidateLinkingOtp, code => Task.FromResult(otp.Validate(code)));
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
            // репорт best-effort; при разрыве теряется
        }
    }

    public async ValueTask DisposeAsync()
    {
        host.Changed -= OnHostChanged;
        await connection.DisposeAsync();
    }

    // одноразовый пароль привязки: один активный код с TTL, проверка одноразовая
    private sealed class OtpState
    {
        private readonly Lock gate = new();
        private (string Value, DateTime Expires)? pending;

        public async Task GenerateAndDeliverAsync(IOtpDelivery delivery)
        {
            var code = Random.Shared.Next(1_000_000).ToString("D6");
            lock (gate) pending = (code, DateTime.UtcNow.AddMinutes(2));
            await delivery.DeliverAsync(code);
        }

        public bool Validate(string code)
        {
            lock (gate)
            {
                if (pending is { } p && p.Value == code && DateTime.UtcNow < p.Expires)
                {
                    pending = null; // одноразовый
                    return true;
                }

                return false;
            }
        }
    }
}
