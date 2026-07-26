using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Properties;

/// <summary>Хендл свойства «освещённость», лк (<see cref="IlluminanceProperty"/>).</summary>
public readonly struct IlluminanceHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal IlluminanceHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Текущее значение, лк, или null, если свойства/значения нет.</summary>
    public async Task<double?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<IlluminanceState>(deviceId, endpointId, IlluminanceProperty.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание свойства или null, если его (или устройства) нет.</summary>
    public Task<IlluminanceProperty?> DescribeAsync(CancellationToken ct = default)
        => host.GetPropertyAsync<IlluminanceProperty>(deviceId, endpointId, IlluminanceProperty.InstanceName, ct);
}
