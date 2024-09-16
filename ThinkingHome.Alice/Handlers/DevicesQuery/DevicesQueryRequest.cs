using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers.DevicesQuery;

public class DevicesQueryRequest
{
    [JsonPropertyName("devices")] public DeviceReference[] Devices { get; set; }
}