namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Установить скорость вентиляции (instance "fan_speed").</summary>
public sealed record FanSpeedCommand : DeviceCommand
{
    public required FanSpeed Value { get; init; }
}
