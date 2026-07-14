using System.Diagnostics.CodeAnalysis;

namespace ThinkingHome.DeviceModel.Remoting.ProxyServer;

/// <summary>
/// Реестр подключённых домашних хостов на прокси. Отдаёт <see cref="IDeviceHost"/> (RemoteHost)
/// по hostId — бридж (Alice/Matter) резолвит hostId из своего контекста (например, пользователя Алисы).
/// </summary>
public interface IRemoteHostRegistry
{
    /// <summary>Вернуть удалённый хост по hostId, если он сейчас подключён.</summary>
    bool TryGet(string hostId, [NotNullWhen(true)] out IDeviceHost? host);

    /// <summary>Идентификаторы подключённых сейчас хостов.</summary>
    IReadOnlyCollection<string> ConnectedHosts { get; }

    event Action<string>? HostConnected;
    event Action<string>? HostDisconnected;
}
