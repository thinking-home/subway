namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущая целевая температура, °C (instance "target_temperature").</summary>
public sealed record TargetTemperatureState : StateValue
{
    /// <summary>Текущая уставка, °C.</summary>
    public required int Value { get; init; }
}
