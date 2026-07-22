namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Установить режим работы термостата/кондиционера (instance "thermostat").</summary>
public sealed record ThermostatModeCommand : DeviceCommand
{
    public required ThermostatMode Value { get; init; }
}
