namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущее состояние осцилляции (instance "oscillation").</summary>
public sealed record OscillationState : StateValue
{
    public required bool Value { get; init; }
}
