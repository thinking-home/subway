using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Capabilities;

/// <summary>Хендл способности «осцилляция» — поворот корпуса вкл/выкл (<see cref="OscillationCapability"/>).</summary>
public readonly struct OscillationHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal OscillationHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Включить/выключить осцилляцию. Способности нет → <see cref="CommandOutcome.Unsupported"/>.</summary>
    public Task<CommandOutcome> SetAsync(bool value, CancellationToken ct = default)
        => host.ExecuteAsync(deviceId, new OscillationCommand
        {
            EndpointId = endpointId,
            Instance = OscillationCapability.InstanceName,
            Value = value,
        }, ct);

    /// <summary>Текущее значение или null, если способности/значения нет.</summary>
    public async Task<bool?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<OscillationState>(deviceId, endpointId, OscillationCapability.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание способности или null, если её (или устройства) нет.</summary>
    public Task<OscillationCapability?> DescribeAsync(CancellationToken ct = default)
        => host.GetCapabilityAsync<OscillationCapability>(deviceId, endpointId, OscillationCapability.InstanceName, ct);
}
