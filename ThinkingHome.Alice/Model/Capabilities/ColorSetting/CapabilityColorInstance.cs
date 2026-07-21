using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.ColorSetting;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CapabilityColorInstance
{
    [JsonStringEnumMemberName("rgb")] Rgb,
    [JsonStringEnumMemberName("temperature_k")] TemperatureK,
}
