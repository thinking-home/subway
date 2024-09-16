using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.OnOff;

public class CapabilityInfoOnOffParams
{
    [JsonPropertyName("split")] public bool Split { get; set; }
}