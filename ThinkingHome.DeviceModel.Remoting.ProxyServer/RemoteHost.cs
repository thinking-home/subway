using Microsoft.AspNetCore.SignalR;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Remoting.ProxyServer;

/// <summary>
/// Прокси-сторона одного домашнего хоста: реализует <see cref="IDeviceHost"/>, но вызовы уходят по
/// SignalR текущему соединению этого hostId. connectionId резолвится на каждый вызов (переживает
/// реконнект). Discovery — pull + кэш (сбрасывается при (ре)коннекте). Живёт в реестре и после
/// разрыва, чтобы подписки бриджа на <see cref="Changed"/> переживали реконнект.
/// </summary>
public sealed class RemoteHost(string hostId, IHubContext<DeviceHub> hub) : IDeviceHost
{
    private volatile string? connectionId;
    private volatile IReadOnlyCollection<DeviceDescriptor>? devicesCache;

    public string HostId => hostId;

    public bool IsOnline => connectionId is not null;

    /// <inheritdoc />
    public event Action<StateChange>? Changed;

    internal void SetConnection(string connId)
    {
        connectionId = connId;
        devicesCache = null; // на (ре)коннекте набор устройств мог измениться — сбрасываем кэш
    }

    internal void ClearConnection(string connId)
    {
        // снимаем только «своё» соединение — не затираем уже переподключившееся
        if (connectionId == connId) connectionId = null;
    }

    internal void RaiseChanged(StateChange change) => Changed?.Invoke(change);

    private ISingleClientProxy Client =>
        connectionId is { } id ? hub.Clients.Client(id) : throw new HostUnavailableException(hostId);

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<DeviceDescriptor>> GetDevicesAsync(CancellationToken ct = default)
    {
        if (devicesCache is { } cached) return cached;

        var devices = await Client.InvokeAsync<DeviceDescriptor[]>(DeviceHubMethods.GetDevices, ct);
        devicesCache = devices;
        return devices;
    }

    /// <inheritdoc />
    public async Task<DeviceDescriptor?> GetDeviceAsync(string deviceId, CancellationToken ct = default)
        => (await GetDevicesAsync(ct)).FirstOrDefault(d => d.Id == deviceId);

    /// <inheritdoc />
    public Task<DeviceSnapshot> QueryAsync(string deviceId, CancellationToken ct = default)
        => Client.InvokeAsync<DeviceSnapshot>(DeviceHubMethods.Query, deviceId, ct);

    /// <inheritdoc />
    public async Task<CommandOutcome> ExecuteAsync(string deviceId, DeviceCommand command, CancellationToken ct = default)
    {
        // у Execute есть канал ошибки — оффлайн отдаём как CommandOutcome, а не исключением (решение 2)
        if (connectionId is null)
        {
            return CommandOutcome.Error(CommandErrorCode.DeviceUnreachable, $"host '{hostId}' offline");
        }

        try
        {
            return await Client.InvokeAsync<CommandOutcome>(DeviceHubMethods.Execute, deviceId, command, ct);
        }
        catch (HostUnavailableException)
        {
            return CommandOutcome.Error(CommandErrorCode.DeviceUnreachable, $"host '{hostId}' offline");
        }
    }

    /// <summary>Привязка: попросить хост сгенерировать и доставить OTP.</summary>
    public Task GenerateLinkingOtpAsync(CancellationToken ct = default)
        => Client.SendAsync(DeviceHubMethods.GenerateLinkingOtp, ct);

    /// <summary>Привязка: проверить OTP на хосте.</summary>
    public Task<bool> ValidateLinkingOtpAsync(string otp, CancellationToken ct = default)
        => Client.InvokeAsync<bool>(DeviceHubMethods.ValidateLinkingOtp, otp, ct);
}
