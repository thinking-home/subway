using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Mode;

public class CapabilityStateModeData
{
    [JsonPropertyName("instance")] public CapabilityModeInstance Instance { get; set; }

    [JsonPropertyName("value")] public CapabilityModeValue Value { get; set; }
}
