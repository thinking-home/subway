using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Properties;

/// <summary>Хендл свойства «индекс качества воздуха» (<see cref="AirQualityProperty"/>).</summary>
public readonly struct AirQualityHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal AirQualityHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Текущее значение или null, если свойства/значения нет.</summary>
    public async Task<AirQuality?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<AirQualityState>(deviceId, endpointId, AirQualityProperty.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание свойства или null, если его (или устройства) нет.</summary>
    public Task<AirQualityProperty?> DescribeAsync(CancellationToken ct = default)
        => host.GetPropertyAsync<AirQualityProperty>(deviceId, endpointId, AirQualityProperty.InstanceName, ct);
}
