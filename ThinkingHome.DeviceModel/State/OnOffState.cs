namespace ThinkingHome.DeviceModel.State;

/// <summary>Значение вкл/выкл (instance "on").</summary>
public sealed record OnOffState : StateValue
{
    public required bool Value { get; init; }
}
