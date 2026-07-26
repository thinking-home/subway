using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Properties;

/// <summary>Хендл свойства «присутствие/движение» (<see cref="OccupancyProperty"/>).</summary>
public readonly struct OccupancyHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal OccupancyHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Текущее значение или null, если свойства/значения нет.</summary>
    public async Task<bool?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<OccupancyState>(deviceId, endpointId, OccupancyProperty.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание свойства или null, если его (или устройства) нет.</summary>
    public Task<OccupancyProperty?> DescribeAsync(CancellationToken ct = default)
        => host.GetPropertyAsync<OccupancyProperty>(deviceId, endpointId, OccupancyProperty.InstanceName, ct);
}
