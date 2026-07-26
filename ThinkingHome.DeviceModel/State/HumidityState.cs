namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущая относительная влажность, % (instance "humidity").</summary>
public sealed record HumidityState : StateValue
{
    /// <summary>Относительная влажность, % (0–100).</summary>
    public required double Value { get; init; }
}
