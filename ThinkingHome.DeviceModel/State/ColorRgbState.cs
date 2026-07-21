namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущий цвет в модели RGB, 0xRRGGBB (instance "color").</summary>
public sealed record ColorRgbState : StateValue
{
    public required int Value { get; init; }
}
