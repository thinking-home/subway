using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Mode;

// Значения режима из фиксированного словаря Алисы. Пока — наборы для fan_speed и thermostat.
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CapabilityModeValue
{
    [JsonStringEnumMemberName("auto")] Auto,
    [JsonStringEnumMemberName("low")] Low,
    [JsonStringEnumMemberName("medium")] Medium,
    [JsonStringEnumMemberName("high")] High,
    [JsonStringEnumMemberName("heat")] Heat,
    [JsonStringEnumMemberName("cool")] Cool,
    [JsonStringEnumMemberName("dry")] Dry,
    [JsonStringEnumMemberName("fan_only")] FanOnly,
}
