namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущая освещённость, лк (instance "illuminance").</summary>
public sealed record IlluminanceState : StateValue
{
    public required double Value { get; init; }
}
