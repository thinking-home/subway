namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущее состояние датчика протечки (instance "water_leak"). true — протечка обнаружена.</summary>
public sealed record WaterLeakState : StateValue
{
    /// <summary>Протечка обнаружена.</summary>
    public required bool Value { get; init; }
}
