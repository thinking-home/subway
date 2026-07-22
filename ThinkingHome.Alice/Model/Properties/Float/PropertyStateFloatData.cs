using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Properties.Float;

public class PropertyStateFloatData
{
    [JsonPropertyName("instance")] public PropertyFloatInstance Instance { get; set; }

    [JsonPropertyName("value")] public float Value { get; set; }
}
