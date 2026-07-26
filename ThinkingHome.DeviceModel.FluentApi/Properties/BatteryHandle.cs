using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Properties;

/// <summary>Хендл свойства «уровень заряда батареи», % (<see cref="BatteryProperty"/>).</summary>
public readonly struct BatteryHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal BatteryHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Текущее значение, %, или null, если свойства/значения нет.</summary>
    public async Task<double?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<BatteryState>(deviceId, endpointId, BatteryProperty.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание свойства или null, если его (или устройства) нет.</summary>
    public Task<BatteryProperty?> DescribeAsync(CancellationToken ct = default)
        => host.GetPropertyAsync<BatteryProperty>(deviceId, endpointId, BatteryProperty.InstanceName, ct);
}
