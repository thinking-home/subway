using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Properties;

/// <summary>Хендл свойства «относительная влажность», % (<see cref="HumidityProperty"/>).</summary>
public readonly struct HumidityHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal HumidityHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Текущее значение, %, или null, если свойства/значения нет.</summary>
    public async Task<double?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<HumidityState>(deviceId, endpointId, HumidityProperty.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание свойства или null, если его (или устройства) нет.</summary>
    public Task<HumidityProperty?> DescribeAsync(CancellationToken ct = default)
        => host.GetPropertyAsync<HumidityProperty>(deviceId, endpointId, HumidityProperty.InstanceName, ct);
}
