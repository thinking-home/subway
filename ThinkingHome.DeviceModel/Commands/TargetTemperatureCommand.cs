namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Установить целевую температуру, °C (instance "target_temperature").</summary>
public sealed record TargetTemperatureCommand : DeviceCommand
{
    /// <summary>Целевая уставка, °C.</summary>
    public required int Value { get; init; }
}
