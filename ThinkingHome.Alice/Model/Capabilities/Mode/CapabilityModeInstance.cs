using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Mode;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CapabilityModeInstance
{
    [JsonStringEnumMemberName("fan_speed")] FanSpeed,
}
