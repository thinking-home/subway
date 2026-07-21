namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Установить цвет в модели RGB, 0xRRGGBB (instance "color").</summary>
public sealed record ColorRgbCommand : DeviceCommand
{
    public required int Value { get; init; }
}
