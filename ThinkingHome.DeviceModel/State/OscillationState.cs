namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущее состояние осцилляции (instance "oscillation").</summary>
public sealed record OscillationState : StateValue
{
    /// <summary>Включена ли осцилляция.</summary>
    public required bool Value { get; init; }
}
