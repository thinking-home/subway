namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущий цвет в модели RGB, 0xRRGGBB (instance "color").</summary>
public sealed record ColorRgbState : StateValue
{
    /// <summary>Цвет, упакованный RGB (0xRRGGBB).</summary>
    public required int Value { get; init; }
}
