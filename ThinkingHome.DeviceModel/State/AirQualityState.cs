namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущий индекс качества воздуха (instance "air_quality").</summary>
public sealed record AirQualityState : StateValue
{
    /// <summary>Индекс качества воздуха.</summary>
    public required AirQuality Value { get; init; }
}
