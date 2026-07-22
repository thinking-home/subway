using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Toggle;

public class CapabilityToggleParams
{
    [JsonPropertyName("instance")] public CapabilityToggleInstance Instance { get; set; }
}
