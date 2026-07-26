namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Установить цветовую температуру в кельвинах (instance "color").</summary>
public sealed record ColorTemperatureCommand : DeviceCommand
{
    /// <summary>Целевая цветовая температура, K.</summary>
    public required int Value { get; init; }
}
