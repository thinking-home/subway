using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Toggle;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CapabilityToggleInstance
{
    [JsonStringEnumMemberName("oscillation")] Oscillation,
}
