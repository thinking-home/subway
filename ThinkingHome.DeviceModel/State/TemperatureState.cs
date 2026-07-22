namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущая температура, °C (instance "temperature").</summary>
public sealed record TemperatureState : StateValue
{
    public required double Value { get; init; }
}
