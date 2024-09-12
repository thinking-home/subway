using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities;

public class CapabilityInfoOnOffParams
{
    [JsonPropertyName("split")] public bool Split { get; set; }
}

public class CapabilityInfoOnOff : CapabilityInfo<CapabilityInfoOnOffParams>
{
}