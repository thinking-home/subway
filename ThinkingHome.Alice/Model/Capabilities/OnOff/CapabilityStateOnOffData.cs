using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.OnOff;

public class CapabilityStateOnOffData
{
    [JsonPropertyName("instance")] public CapabilityStateOnOffInstance Instance { get; set; }

    [JsonPropertyName("value")] public bool Value { get; set; }
}