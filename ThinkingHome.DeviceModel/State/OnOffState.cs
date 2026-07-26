namespace ThinkingHome.DeviceModel.State;

/// <summary>Значение вкл/выкл (instance "on").</summary>
public sealed record OnOffState : StateValue
{
    /// <summary>Текущее состояние: true — включено.</summary>
    public required bool Value { get; init; }
}
