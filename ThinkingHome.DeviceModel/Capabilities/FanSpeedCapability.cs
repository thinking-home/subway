namespace ThinkingHome.DeviceModel.Capabilities;

/// <summary>Скорость вентиляции (выбор из набора). Matter cluster Fan Control (0x0202). Instance — "fan_speed".</summary>
public sealed record FanSpeedCapability : Capability
{
    /// <summary>Поддерживаемые устройством скорости.</summary>
    public required IReadOnlyList<FanSpeed> Speeds { get; init; }
}
