using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.ColorSetting;

/// <summary>Состояние color_setting: инстанс и целочисленное значение.</summary>
public class CapabilityStateColorData
{
    /// <summary>Инстанс: rgb или temperature_k.</summary>
    [JsonPropertyName("instance")] public CapabilityColorInstance Instance { get; set; }

    /// <summary>Значение: и rgb (0xRRGGBB), и temperature_k (кельвины) — целые числа.</summary>
    [JsonPropertyName("value")] public int Value { get; set; }
}
