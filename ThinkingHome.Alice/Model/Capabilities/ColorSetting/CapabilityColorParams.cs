using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.ColorSetting;

// Параметры color_setting в discovery: rgb-модель и/или диапазон цветовой температуры (одно из/оба).
public class CapabilityColorParams
{
    [JsonPropertyName("color_model")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ColorModel { get; set; }

    [JsonPropertyName("temperature_k")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CapabilityColorTemperatureRange TemperatureK { get; set; }
}

public class CapabilityColorTemperatureRange
{
    [JsonPropertyName("min")] public int Min { get; set; }
    [JsonPropertyName("max")] public int Max { get; set; }
}
