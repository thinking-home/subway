using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Range;

public class CapabilityStateRangeData
{
    [JsonPropertyName("instance")] public CapabilityStateRangeInstance Instance { get; set; }

    [JsonPropertyName("value")] public float Value { get; set; }
}
