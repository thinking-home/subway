namespace ThinkingHome.DeviceModel.Properties;

/// <summary>Индекс качества воздуха (перечисление). Обязательный кластер типа Air Quality Sensor — Matter cluster Air Quality (0x005B). Instance — "air_quality".</summary>
public sealed record AirQualityProperty : Property
{
    /// <summary>Канонический instance.</summary>
    public const string InstanceName = "air_quality";
}
