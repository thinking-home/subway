namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущий индекс качества воздуха (instance "air_quality").</summary>
public sealed record AirQualityState : StateValue
{
    public required AirQuality Value { get; init; }
}
