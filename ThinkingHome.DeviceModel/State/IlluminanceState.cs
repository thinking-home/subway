namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущая освещённость, лк (instance "illuminance").</summary>
public sealed record IlluminanceState : StateValue
{
    /// <summary>Освещённость, лк.</summary>
    public required double Value { get; init; }
}
