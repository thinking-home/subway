using System.Text.Json.Serialization;

namespace ThinkingHome.DeviceModel;

/// <summary>
/// Индекс качества воздуха — значения атрибута AirQuality кластера Air Quality (0x005B) Matter.
/// Значение Unknown из Matter не переносится: «нет данных» в модели выражается отсутствием значения.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AirQuality
{
    /// <summary>Хорошее.</summary>
    Good,
    /// <summary>Приемлемое.</summary>
    Fair,
    /// <summary>Умеренно загрязнённое.</summary>
    Moderate,
    /// <summary>Плохое.</summary>
    Poor,
    /// <summary>Очень плохое.</summary>
    VeryPoor,
    /// <summary>Крайне плохое.</summary>
    ExtremelyPoor,
}
