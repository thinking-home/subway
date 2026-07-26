using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.ColorSetting;

/// <summary>Инстанс состояния color_setting — какая часть настройки цвета меняется.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CapabilityColorInstance
{
    /// <summary>Цвет в модели RGB.</summary>
    [JsonStringEnumMemberName("rgb")] Rgb,
    /// <summary>Цветовая температура в кельвинах.</summary>
    [JsonStringEnumMemberName("temperature_k")] TemperatureK,
}
