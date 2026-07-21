namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Установить цветовую температуру в кельвинах (instance "temperature_k").</summary>
public sealed record ColorTemperatureCommand : DeviceCommand
{
    public required int Value { get; init; }
}
