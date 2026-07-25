namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущее атмосферное давление, кПа (instance "pressure").</summary>
public sealed record PressureState : StateValue
{
    public required double Value { get; init; }
}
