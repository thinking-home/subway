using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Toggle;

public class CapabilityStateToggleData
{
    [JsonPropertyName("instance")] public CapabilityToggleInstance Instance { get; set; }

    [JsonPropertyName("value")] public bool Value { get; set; }
}
