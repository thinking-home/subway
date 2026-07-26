using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi.Capabilities;

/// <summary>
/// Хендл способности «цвет» (<see cref="ColorCapability"/>). Представления взаимоисключающие и
/// делят один слот (instance "color"): активен RGB → <see cref="GetTemperatureAsync"/> вернёт null,
/// и наоборот.
/// </summary>
public readonly struct ColorHandle
{
    private readonly IDeviceHost host;
    private readonly string deviceId;
    private readonly int endpointId;

    internal ColorHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        this.deviceId = deviceId;
        this.endpointId = endpointId;
    }

    /// <summary>Установить полный цвет, упакованный RGB (0xRRGGBB).</summary>
    public Task<CommandOutcome> SetRgbAsync(int rgb, CancellationToken ct = default)
        => host.ExecuteAsync(deviceId, new ColorRgbCommand
        {
            EndpointId = endpointId,
            Instance = ColorCapability.InstanceName,
            Value = rgb,
        }, ct);

    /// <summary>Установить цветовую температуру, K.</summary>
    public Task<CommandOutcome> SetTemperatureAsync(int kelvin, CancellationToken ct = default)
        => host.ExecuteAsync(deviceId, new ColorTemperatureCommand
        {
            EndpointId = endpointId,
            Instance = ColorCapability.InstanceName,
            Value = kelvin,
        }, ct);

    /// <summary>Текущий RGB или null (способности нет либо активно другое представление).</summary>
    public async Task<int?> GetRgbAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<ColorRgbState>(deviceId, endpointId, ColorCapability.InstanceName, ct))?.Value;

    /// <summary>Текущая цветовая температура, K, или null (способности нет либо активно другое представление).</summary>
    public async Task<int?> GetTemperatureAsync(CancellationToken ct = default)
        => (await host.GetStateAsync<ColorTemperatureState>(deviceId, endpointId, ColorCapability.InstanceName, ct))?.Value;

    /// <summary>Discovery: описание способности (какие представления поддержаны) или null.</summary>
    public Task<ColorCapability?> DescribeAsync(CancellationToken ct = default)
        => host.GetCapabilityAsync<ColorCapability>(deviceId, endpointId, ColorCapability.InstanceName, ct);
}
