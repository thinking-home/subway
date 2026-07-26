namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Установить режим работы термостата/кондиционера (instance "thermostat").</summary>
public sealed record ThermostatModeCommand : DeviceCommand
{
    /// <summary>Целевой режим термостата.</summary>
    public required ThermostatMode Value { get; init; }
}
