using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Properties;

/// <summary>Хендл свойства «накопленные показания счётчика воды», м³ (<see cref="WaterMeterProperty"/>).</summary>
public readonly struct WaterMeterHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal WaterMeterHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Текущие показания, м³, или null, если свойства/значения нет.</summary>
    public async Task<double?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<WaterMeterState>(deviceId, endpointId, WaterMeterProperty.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание свойства или null, если его (или устройства) нет.</summary>
    public Task<WaterMeterProperty?> DescribeAsync(CancellationToken ct = default)
        => host.GetPropertyAsync<WaterMeterProperty>(deviceId, endpointId, WaterMeterProperty.InstanceName, ct);
}
