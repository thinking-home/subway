namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Установить цвет в модели RGB, 0xRRGGBB (instance "color").</summary>
public sealed record ColorRgbCommand : DeviceCommand
{
    /// <summary>Целевой цвет, упакованный RGB (0xRRGGBB).</summary>
    public required int Value { get; init; }
}
