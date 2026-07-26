using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Properties;

/// <summary>Хендл свойства «протечка воды» — true, если протечка обнаружена (<see cref="WaterLeakProperty"/>).</summary>
public readonly struct WaterLeakHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal WaterLeakHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Текущее значение или null, если свойства/значения нет.</summary>
    public async Task<bool?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<WaterLeakState>(deviceId, endpointId, WaterLeakProperty.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание свойства или null, если его (или устройства) нет.</summary>
    public Task<WaterLeakProperty?> DescribeAsync(CancellationToken ct = default)
        => host.GetPropertyAsync<WaterLeakProperty>(deviceId, endpointId, WaterLeakProperty.InstanceName, ct);
}
