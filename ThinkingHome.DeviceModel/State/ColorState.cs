namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущий цвет RGB, 0xRRGGBB (instance "rgb").</summary>
public sealed record ColorState : StateValue
{
    public required int Value { get; init; }
}
