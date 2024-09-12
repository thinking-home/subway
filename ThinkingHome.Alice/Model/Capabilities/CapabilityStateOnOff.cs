using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CapabilityStateOnOffInstance
{
    [JsonStringEnumMemberName("on")] On,
}

public class CapabilityStateOnOffData
{
    [JsonPropertyName("instance")] public CapabilityStateOnOffInstance Instance { get; set; }

    [JsonPropertyName("value")] public bool Value { get; set; }
}

public class CapabilityStateOnOff : CapabilityState<CapabilityStateOnOffData>
{
}