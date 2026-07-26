using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Capabilities;

/// <summary>Хендл способности «режим термостата/кондиционера» (<see cref="ThermostatModeCapability"/>).</summary>
public readonly struct ThermostatModeHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal ThermostatModeHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Установить режим. Способности нет → <see cref="CommandOutcome.Unsupported"/>.</summary>
    public Task<CommandOutcome> SetAsync(ThermostatMode value, CancellationToken ct = default)
        => host.ExecuteAsync(deviceId, new ThermostatModeCommand
        {
            EndpointId = endpointId,
            Instance = ThermostatModeCapability.InstanceName,
            Value = value,
        }, ct);

    /// <summary>Текущее значение или null, если способности/значения нет.</summary>
    public async Task<ThermostatMode?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<ThermostatModeState>(deviceId, endpointId, ThermostatModeCapability.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание способности (поддерживаемые режимы) или null.</summary>
    public Task<ThermostatModeCapability?> DescribeAsync(CancellationToken ct = default)
        => host.GetCapabilityAsync<ThermostatModeCapability>(deviceId, endpointId, ThermostatModeCapability.InstanceName, ct);
}
