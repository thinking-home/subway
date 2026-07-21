using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.ColorSetting;

public class CapabilityStateColorData
{
    [JsonPropertyName("instance")] public CapabilityColorInstance Instance { get; set; }

    // и rgb (0xRRGGBB), и temperature_k (кельвины) — целые числа
    [JsonPropertyName("value")] public int Value { get; set; }
}
