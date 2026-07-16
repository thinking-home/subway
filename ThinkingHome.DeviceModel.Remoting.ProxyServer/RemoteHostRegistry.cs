using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.SignalR;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Remoting.ProxyServer;

/// <summary>
/// Реестр удалённых хостов на прокси: держит карты hostId → RemoteHost и connectionId → hostId,
/// обновляет их на connect/disconnect (политика last-wins) и роутит входящие Report'ы в нужный хост.
/// RemoteHost для hostId не удаляется при разрыве — просто уходит в offline, чтобы подписки бриджа жили.
/// </summary>
public sealed class RemoteHostRegistry(IHubContext<DeviceHub> hub) : IRemoteHostRegistry
{
    private readonly ConcurrentDictionary<string, RemoteHost> hostsById = new();
    private readonly ConcurrentDictionary<string, string> hostIdByConnection = new();

    public event Action<string>? HostConnected;
    public event Action<string>? HostDisconnected;

    public IReadOnlyCollection<string> ConnectedHosts =>
        hostsById.Values.Where(h => h.IsOnline).Select(h => h.HostId).ToArray();

    internal void Attach(string hostId, string connectionId)
    {
        hostIdByConnection[connectionId] = hostId;
        var host = hostsById.GetOrAdd(hostId, id => new RemoteHost(id, hub));
        host.SetConnection(connectionId); // last-wins: новое соединение вытесняет старое
        HostConnected?.Invoke(hostId);
    }

    internal void Detach(string connectionId)
    {
        if (!hostIdByConnection.TryRemove(connectionId, out var hostId)) return;
        if (hostsById.TryGetValue(hostId, out var host))
        {
            host.ClearConnection(connectionId);
            HostDisconnected?.Invoke(hostId);
        }
    }

    internal void DispatchReport(string connectionId, StateChange change)
    {
        if (hostIdByConnection.TryGetValue(connectionId, out var hostId)
            && hostsById.TryGetValue(hostId, out var host))
        {
            host.RaiseChanged(change);
        }
    }

    public bool TryGet(string hostId, [NotNullWhen(true)] out IDeviceHost? host)
    {
        if (hostsById.TryGetValue(hostId, out var h) && h.IsOnline)
        {
            host = h;
            return true;
        }

        host = null;
        return false;
    }

    // OTP-привязка: маршрутизируем на онлайн-хост по hostId (состояние OTP — на хосте, прокси stateless)
    public Task GenerateLinkingOtpAsync(string hostId, CancellationToken ct = default)
        => RequiredOnline(hostId).GenerateLinkingOtpAsync(ct);

    public Task<bool> ValidateLinkingOtpAsync(string hostId, string otp, CancellationToken ct = default)
        => RequiredOnline(hostId).ValidateLinkingOtpAsync(otp, ct);

    private RemoteHost RequiredOnline(string hostId)
        => hostsById.TryGetValue(hostId, out var host) && host.IsOnline
            ? host
            : throw new HostUnavailableException(hostId);
}
