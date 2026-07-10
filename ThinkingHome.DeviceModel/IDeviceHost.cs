namespace ThinkingHome.DeviceModel;

/// <summary>
/// Потребитель-грань хоста — публичный .NET API слоя устройств. Его вызывают адаптеры экосистем
/// (Алиса, Matter, HomeKit) и код ThinkingHome. Экосистемные адаптеры не лезут в реестр напрямую.
/// </summary>
public interface IDeviceHost
{
    /// <summary>Все зарегистрированные устройства (discovery).</summary>
    IReadOnlyCollection<DeviceDescriptor> GetDevices();

    /// <summary>Описание одного устройства или null, если его нет.</summary>
    DeviceDescriptor? GetDevice(string deviceId);

    /// <summary>Опросить состояние устройства (query).</summary>
    Task<DeviceSnapshot> QueryAsync(string deviceId, CancellationToken ct = default);

    /// <summary>Исполнить команду над устройством (execute).</summary>
    Task<CommandOutcome> ExecuteAsync(string deviceId, DeviceCommand command, CancellationToken ct = default);

    /// <summary>Агрегированный поток изменений состояния всех устройств (report).</summary>
    event Action<StateChange> Changed;
}
