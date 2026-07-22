using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Properties.Float;

public class PropertyFloatParams
{
    [JsonPropertyName("instance")] public PropertyFloatInstance Instance { get; set; }

    [JsonPropertyName("unit")] public string Unit { get; set; }
}
