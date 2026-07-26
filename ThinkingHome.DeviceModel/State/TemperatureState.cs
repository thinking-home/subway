namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущая температура, °C (instance "temperature").</summary>
public sealed record TemperatureState : StateValue
{
    /// <summary>Температура, °C.</summary>
    public required double Value { get; init; }
}
