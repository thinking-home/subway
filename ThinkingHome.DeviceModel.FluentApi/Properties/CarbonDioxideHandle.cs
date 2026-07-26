using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Properties;

/// <summary>Хендл свойства «концентрация CO2», ppm (<see cref="CarbonDioxideProperty"/>).</summary>
public readonly struct CarbonDioxideHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal CarbonDioxideHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Текущее значение, ppm, или null, если свойства/значения нет.</summary>
    public async Task<double?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<CarbonDioxideState>(deviceId, endpointId, CarbonDioxideProperty.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание свойства или null, если его (или устройства) нет.</summary>
    public Task<CarbonDioxideProperty?> DescribeAsync(CancellationToken ct = default)
        => host.GetPropertyAsync<CarbonDioxideProperty>(deviceId, endpointId, CarbonDioxideProperty.InstanceName, ct);
}
