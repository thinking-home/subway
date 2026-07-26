namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущий уровень заряда батареи, % (instance "battery").</summary>
public sealed record BatteryState : StateValue
{
    /// <summary>Уровень заряда батареи, % (0–100).</summary>
    public required double Value { get; init; }
}
