using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Properties.Event;

public class PropertyEventParams
{
    [JsonPropertyName("instance")] public PropertyEventInstance Instance { get; set; }

    [JsonPropertyName("events")] public PropertyEventOption[] Events { get; set; }
}

public class PropertyEventOption
{
    [JsonPropertyName("value")] public PropertyEventValue Value { get; set; }
}
