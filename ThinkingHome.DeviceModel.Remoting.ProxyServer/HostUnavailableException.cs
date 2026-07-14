namespace ThinkingHome.DeviceModel.Remoting.ProxyServer;

/// <summary>
/// Домашний хост с таким hostId сейчас не подключён к прокси. Бросается методами без канала ошибки
/// (<c>GetDevicesAsync</c>/<c>QueryAsync</c>); бридж ловит и мапит в ошибку своей экосистемы.
/// У <c>ExecuteAsync</c> оффлайн возвращается как <see cref="CommandOutcome"/> с кодом
/// <see cref="CommandErrorCode.DeviceUnreachable"/>.
/// </summary>
public sealed class HostUnavailableException(string hostId)
    : Exception($"Device host '{hostId}' is not connected.")
{
    public string HostId { get; } = hostId;
}
