using System.Collections.Concurrent;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel;

/// <summary>
/// Реализация хоста устройств: сразу обе грани — <see cref="IDeviceRegistry"/> (регистрация драйверов)
/// и <see cref="IDeviceHost"/> (потребительский .NET API). Хранит карту id → <see cref="DeviceEntry"/>
/// и агрегированное событие изменений. Всё, что касается конкретного устройства (кэш, подписка на его
/// изменения), инкапсулировано в <see cref="DeviceEntry"/>; хост общается с записью только через её
/// публичный интерфейс, а записи передаёт два колбэка — сама запись про хост не знает.
/// </summary>
public sealed class DeviceHost : IDeviceRegistry, IDeviceHost
{
    private readonly ConcurrentDictionary<string, DeviceEntry> entries = new();

    /// <inheritdoc />
    public event Action<StateChange>? Changed;

    #region IDeviceRegistry (провайдер-грань)

    /// <inheritdoc />
    public IDisposable Register(IDevice device)
    {
        var entry = new DeviceEntry(device, RaiseChanged, Remove);

        if (!entries.TryAdd(device.Id, entry))
        {
            // запись ещё не подписана на устройство — чистить нечего, просто отказываемся
            throw new InvalidOperationException($"Device '{device.Id}' is already registered.");
        }

        entry.Attach();
        return entry;
    }

    #endregion

    #region IDeviceHost (потребитель-грань)

    /// <inheritdoc />
    public Task<IReadOnlyCollection<DeviceDescriptor>> GetDevicesAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyCollection<DeviceDescriptor>>(
            entries.Values.Select(entry => entry.Describe()).ToArray());

    /// <inheritdoc />
    public Task<DeviceDescriptor?> GetDeviceAsync(string deviceId, CancellationToken ct = default)
        => Task.FromResult(entries.TryGetValue(deviceId, out var entry) ? entry.Describe() : null);

    /// <inheritdoc />
    public Task<DeviceSnapshot> QueryAsync(string deviceId, CancellationToken ct = default)
        => GetRequired(deviceId).QueryAsync(ct);

    /// <inheritdoc />
    public Task<CommandOutcome> ExecuteAsync(string deviceId, DeviceCommand command, CancellationToken ct = default)
        => GetRequired(deviceId).ExecuteAsync(command, ct);

    #endregion

    private DeviceEntry GetRequired(string deviceId)
        => entries.TryGetValue(deviceId, out var entry)
            ? entry
            : throw new KeyNotFoundException($"Device '{deviceId}' is not registered.");

    // колбэки, которые хост передаёт каждой записи; запись про хост ничего не знает
    private void RaiseChanged(StateChange change) => Changed?.Invoke(change);

    private void Remove(DeviceEntry entry)
        => entries.TryRemove(new KeyValuePair<string, DeviceEntry>(entry.Id, entry));
}
