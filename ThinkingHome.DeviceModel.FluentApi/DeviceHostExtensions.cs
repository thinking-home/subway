using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi;

/// <summary>
/// Вход во fluent API — сахар над <see cref="IDeviceHost"/>. Любая цепочка сводится
/// к пяти членам ядра, поэтому одинаково работает с локальным хостом и с прокси на хабе и не
/// добавляет семантики: каждый метод — ровно один вызов ядра.
/// </summary>
public static class DeviceHostExtensions
{
    /// <summary>
    /// Хендл устройства — адрес, а не снимок: без I/O и без проверки существования (она всё равно
    /// ничего не гарантирует к моменту вызова). Отсутствие устройства обнаруживается на вызовах.
    /// </summary>
    public static DeviceHandle Device(this IDeviceHost host, string deviceId) => new(host, deviceId);

    /// <summary>Подписка на изменения всех устройств хоста; отписка — Dispose.</summary>
    public static IDisposable OnChanged(this IDeviceHost host, Action<StateChange> handler)
        => new ChangedSubscription(host, handler);
}
