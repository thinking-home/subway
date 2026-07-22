namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущее состояние датчика протечки (instance "water_leak"). true — протечка обнаружена.</summary>
public sealed record WaterLeakState : StateValue
{
    public required bool Value { get; init; }
}
