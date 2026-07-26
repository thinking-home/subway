using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Properties;

/// <summary>
/// Хендл свойства «контакт датчика открытия» (<see cref="ContactProperty"/>).
/// Семантика Matter: true — контакт замкнут, т.е. закрыто.
/// </summary>
public readonly struct ContactHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal ContactHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Текущее значение или null, если свойства/значения нет.</summary>
    public async Task<bool?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<ContactState>(deviceId, endpointId, ContactProperty.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание свойства или null, если его (или устройства) нет.</summary>
    public Task<ContactProperty?> DescribeAsync(CancellationToken ct = default)
        => host.GetPropertyAsync<ContactProperty>(deviceId, endpointId, ContactProperty.InstanceName, ct);
}
