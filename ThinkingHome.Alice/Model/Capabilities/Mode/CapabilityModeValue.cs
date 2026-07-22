using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Mode;

// Значения режима из фиксированного словаря Алисы. Пока — набор для fan_speed.
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CapabilityModeValue
{
    [JsonStringEnumMemberName("auto")] Auto,
    [JsonStringEnumMemberName("low")] Low,
    [JsonStringEnumMemberName("medium")] Medium,
    [JsonStringEnumMemberName("high")] High,
}
