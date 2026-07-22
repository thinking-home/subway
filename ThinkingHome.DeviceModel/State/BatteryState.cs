namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущий уровень заряда батареи, % (instance "battery").</summary>
public sealed record BatteryState : StateValue
{
    public required double Value { get; init; }
}
