namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущая степень открытия 0–100 % (instance "open").</summary>
public sealed record OpenState : StateValue
{
    public required int Value { get; init; }
}
