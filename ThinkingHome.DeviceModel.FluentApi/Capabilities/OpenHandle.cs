using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Capabilities;

/// <summary>Хендл способности «степень открытия», 0–100 % (0 — закрыто; <see cref="OpenCapability"/>).</summary>
public readonly struct OpenHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal OpenHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Установить степень открытия, %. Способности нет → <see cref="CommandOutcome.Unsupported"/>.</summary>
    public Task<CommandOutcome> SetAsync(int value, CancellationToken ct = default)
        => host.ExecuteAsync(deviceId, new OpenCommand
        {
            EndpointId = endpointId,
            Instance = OpenCapability.InstanceName,
            Value = value,
        }, ct);

    /// <summary>Текущее значение или null, если способности/значения нет.</summary>
    public async Task<int?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<OpenState>(deviceId, endpointId, OpenCapability.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание способности или null, если её (или устройства) нет.</summary>
    public Task<OpenCapability?> DescribeAsync(CancellationToken ct = default)
        => host.GetCapabilityAsync<OpenCapability>(deviceId, endpointId, OpenCapability.InstanceName, ct);
}
