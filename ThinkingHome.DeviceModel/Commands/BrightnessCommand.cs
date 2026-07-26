namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Установить яркость 0–100 % (instance "brightness").</summary>
public sealed record BrightnessCommand : DeviceCommand
{
    /// <summary>Целевая яркость, % (0–100).</summary>
    public required int Value { get; init; }
}
