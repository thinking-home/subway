using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Capabilities;

/// <summary>Хендл способности «целевая температура» — уставка, °C (<see cref="TargetTemperatureCapability"/>).</summary>
public readonly struct TargetTemperatureHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal TargetTemperatureHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Установить уставку, °C. Способности нет → <see cref="CommandOutcome.Unsupported"/>.</summary>
    public Task<CommandOutcome> SetAsync(int value, CancellationToken ct = default)
        => host.ExecuteAsync(deviceId, new TargetTemperatureCommand
        {
            EndpointId = endpointId,
            Instance = TargetTemperatureCapability.InstanceName,
            Value = value,
        }, ct);

    /// <summary>Текущее значение или null, если способности/значения нет.</summary>
    public async Task<int?> GetAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<TargetTemperatureState>(deviceId, endpointId, TargetTemperatureCapability.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание способности (диапазон уставки) или null.</summary>
    public Task<TargetTemperatureCapability?> DescribeAsync(CancellationToken ct = default)
        => host.GetCapabilityAsync<TargetTemperatureCapability>(deviceId, endpointId, TargetTemperatureCapability.InstanceName, ct);
}
