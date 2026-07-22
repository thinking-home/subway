using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Properties.Event;

public class PropertyStateEventData
{
    [JsonPropertyName("instance")] public PropertyEventInstance Instance { get; set; }

    [JsonPropertyName("value")] public PropertyEventValue Value { get; set; }
}
