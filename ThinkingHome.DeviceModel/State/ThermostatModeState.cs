namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущий режим работы термостата/кондиционера (instance "thermostat").</summary>
public sealed record ThermostatModeState : StateValue
{
    public required ThermostatMode Value { get; init; }
}
