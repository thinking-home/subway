using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Capabilities;

/// <summary>Хендл способности «скорость вентиляции» (<see cref="FanSpeedCapability"/>).</summary>
public readonly struct FanSpeedHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal FanSpeedHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Установить скорость. Способности нет → <see cref="CommandOutcome.Unsupported"/>.</summary>
    public Task<CommandOutcome> SetAsync(FanSpeed value, CancellationToken ct = default)
        => host.ExecuteAsync(deviceId, new FanSpeedCommand
        {
            EndpointId = endpointId,
            Instance = FanSpeedCapability.InstanceName,
            Value = value,
        }, ct);

    /// <summary>Текущее значение или null, если способности/значения нет.</summary>
    public async Task<FanSpeed?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<FanSpeedState>(deviceId, endpointId, FanSpeedCapability.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание способности (поддерживаемые скорости) или null.</summary>
    public Task<FanSpeedCapability?> DescribeAsync(CancellationToken ct = default)
        => host.GetCapabilityAsync<FanSpeedCapability>(deviceId, endpointId, FanSpeedCapability.InstanceName, ct);
}
