namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущая степень открытия 0–100 % (instance "open").</summary>
public sealed record OpenState : StateValue
{
    /// <summary>Степень открытия, % (0 — закрыто, 100 — открыто).</summary>
    public required int Value { get; init; }
}
