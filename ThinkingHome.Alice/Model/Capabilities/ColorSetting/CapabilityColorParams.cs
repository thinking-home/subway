using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.ColorSetting;

/// <summary>Параметры color_setting в discovery: rgb-модель и/или диапазон цветовой температуры (одно из/оба).</summary>
public class CapabilityColorParams
{
    /// <summary>Цветовая модель (константа из <see cref="ColorModels"/>); null — управление цветом не заявляется.</summary>
    [JsonPropertyName("color_model")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ColorModel { get; set; }

    /// <summary>Диапазон цветовой температуры в кельвинах; null — не поддерживается.</summary>
    [JsonPropertyName("temperature_k")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CapabilityColorTemperatureRange TemperatureK { get; set; }
}

/// <summary>Диапазон цветовой температуры в кельвинах.</summary>
public class CapabilityColorTemperatureRange
{
    /// <summary>Минимальная цветовая температура, K.</summary>
    [JsonPropertyName("min")] public int Min { get; set; }
    /// <summary>Максимальная цветовая температура, K.</summary>
    [JsonPropertyName("max")] public int Max { get; set; }
}
