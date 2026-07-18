using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Range;

public class CapabilityRangeParams
{
    [JsonPropertyName("instance")] public CapabilityStateRangeInstance Instance { get; set; }

    [JsonPropertyName("unit")] public string Unit { get; set; }

    [JsonPropertyName("random_access")] public bool RandomAccess { get; set; }

    [JsonPropertyName("range")] public CapabilityRangeLimits Range { get; set; }
}

public class CapabilityRangeLimits
{
    [JsonPropertyName("min")] public float Min { get; set; }
    [JsonPropertyName("max")] public float Max { get; set; }
    [JsonPropertyName("precision")] public float Precision { get; set; }
}
