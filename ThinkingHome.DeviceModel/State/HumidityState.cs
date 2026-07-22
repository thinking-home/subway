namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущая относительная влажность, % (instance "humidity").</summary>
public sealed record HumidityState : StateValue
{
    public required double Value { get; init; }
}
