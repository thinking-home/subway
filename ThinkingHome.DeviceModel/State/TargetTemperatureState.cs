namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущая целевая температура, °C (instance "temperature").</summary>
public sealed record TargetTemperatureState : StateValue
{
    public required int Value { get; init; }
}
