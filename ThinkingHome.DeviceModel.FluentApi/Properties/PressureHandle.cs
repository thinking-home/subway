using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Properties;

/// <summary>Хендл свойства «атмосферное давление», кПа (<see cref="PressureProperty"/>).</summary>
public readonly struct PressureHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal PressureHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Текущее значение, кПа, или null, если свойства/значения нет.</summary>
    public async Task<double?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<PressureState>(deviceId, endpointId, PressureProperty.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание свойства или null, если его (или устройства) нет.</summary>
    public Task<PressureProperty?> DescribeAsync(CancellationToken ct = default)
        => host.GetPropertyAsync<PressureProperty>(deviceId, endpointId, PressureProperty.InstanceName, ct);
}
