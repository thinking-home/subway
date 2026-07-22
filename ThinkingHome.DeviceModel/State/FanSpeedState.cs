namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущая скорость вентиляции (instance "fan_speed").</summary>
public sealed record FanSpeedState : StateValue
{
    public required FanSpeed Value { get; init; }
}
