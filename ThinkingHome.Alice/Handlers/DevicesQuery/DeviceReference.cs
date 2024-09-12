using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers.DevicesQuery
{
    public class DeviceReference
    {
        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("custom_data")] public object CustomData { get; set; }
    }
}