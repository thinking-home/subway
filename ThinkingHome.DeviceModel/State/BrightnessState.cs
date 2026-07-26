namespace ThinkingHome.DeviceModel.State;

/// <summary>Значение яркости 0–100 % (instance "brightness").</summary>
public sealed record BrightnessState : StateValue
{
    /// <summary>Яркость, % (0–100).</summary>
    public required int Value { get; init; }
}
