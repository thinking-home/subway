namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Установить яркость 0–100 % (instance "brightness").</summary>
public sealed record BrightnessCommand : DeviceCommand
{
    public required int Value { get; init; }
}
