using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Properties;

/// <summary>Хендл свойства «температура», °C (<see cref="TemperatureProperty"/>).</summary>
public readonly struct TemperatureHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal TemperatureHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Текущее значение, °C, или null, если свойства/значения нет.</summary>
    public async Task<double?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<TemperatureState>(deviceId, endpointId, TemperatureProperty.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание свойства или null, если его (или устройства) нет.</summary>
    public Task<TemperatureProperty?> DescribeAsync(CancellationToken ct = default)
        => host.GetPropertyAsync<TemperatureProperty>(deviceId, endpointId, TemperatureProperty.InstanceName, ct);
}
