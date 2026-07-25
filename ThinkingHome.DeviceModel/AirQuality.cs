using System.Text.Json.Serialization;

namespace ThinkingHome.DeviceModel;

/// <summary>
/// Индекс качества воздуха — значения атрибута AirQuality кластера Air Quality (0x005B) Matter.
/// Значение Unknown из Matter не переносится: «нет данных» в модели выражается отсутствием значения.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AirQuality
{
    Good,
    Fair,
    Moderate,
    Poor,
    VeryPoor,
    ExtremelyPoor,
}
