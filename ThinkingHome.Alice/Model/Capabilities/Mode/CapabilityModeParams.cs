using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Mode;

public class CapabilityModeParams
{
    [JsonPropertyName("instance")] public CapabilityModeInstance Instance { get; set; }

    [JsonPropertyName("modes")] public CapabilityModeOption[] Modes { get; set; }
}

public class CapabilityModeOption
{
    [JsonPropertyName("value")] public CapabilityModeValue Value { get; set; }
}
