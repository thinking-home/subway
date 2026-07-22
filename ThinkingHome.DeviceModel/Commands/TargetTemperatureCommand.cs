namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Установить целевую температуру, °C (instance "temperature").</summary>
public sealed record TargetTemperatureCommand : DeviceCommand
{
    public required int Value { get; init; }
}
