namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Установить скорость вентиляции (instance "fan_speed").</summary>
public sealed record FanSpeedCommand : DeviceCommand
{
    /// <summary>Целевая скорость вентиляции.</summary>
    public required FanSpeed Value { get; init; }
}
