using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Capabilities;

/// <summary>Хендл способности вкл/выкл (<see cref="OnOffCapability"/>).</summary>
public readonly struct OnOffHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal OnOffHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Включить; синоним <c>SetAsync(true)</c>.</summary>
    public Task<CommandOutcome> TurnOnAsync(CancellationToken ct = default) => SetAsync(true, ct);

    /// <summary>Выключить; синоним <c>SetAsync(false)</c>.</summary>
    public Task<CommandOutcome> TurnOffAsync(CancellationToken ct = default) => SetAsync(false, ct);

    /// <summary>Включить/выключить. Способности нет → <see cref="CommandOutcome.Unsupported"/>.</summary>
    public Task<CommandOutcome> SetAsync(bool value, CancellationToken ct = default)
        => host.ExecuteAsync(deviceId, new OnOffCommand
        {
            EndpointId = endpointId,
            Instance = OnOffCapability.InstanceName,
            Value = value,
        }, ct);

    /// <summary>Текущее значение или null, если способности/значения нет.</summary>
    public async Task<bool?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<OnOffState>(deviceId, endpointId, OnOffCapability.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание способности или null, если её (или устройства) нет.</summary>
    public Task<OnOffCapability?> DescribeAsync(CancellationToken ct = default)
        => host.GetCapabilityAsync<OnOffCapability>(deviceId, endpointId, OnOffCapability.InstanceName, ct);
}
