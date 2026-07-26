using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Capabilities;

/// <summary>Хендл способности «яркость», 0–100 % (<see cref="BrightnessCapability"/>).</summary>
public readonly struct BrightnessHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal BrightnessHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Установить яркость, %. Способности нет → <see cref="CommandOutcome.Unsupported"/>.</summary>
    public Task<CommandOutcome> SetAsync(int value, CancellationToken ct = default)
        => host.ExecuteAsync(deviceId, new BrightnessCommand
        {
            EndpointId = endpointId,
            Instance = BrightnessCapability.InstanceName,
            Value = value,
        }, ct);

    /// <summary>Текущее значение или null, если способности/значения нет.</summary>
    public async Task<int?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<BrightnessState>(deviceId, endpointId, BrightnessCapability.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание способности или null, если её (или устройства) нет.</summary>
    public Task<BrightnessCapability?> DescribeAsync(CancellationToken ct = default)
        => host.GetCapabilityAsync<BrightnessCapability>(deviceId, endpointId, BrightnessCapability.InstanceName, ct);
}
