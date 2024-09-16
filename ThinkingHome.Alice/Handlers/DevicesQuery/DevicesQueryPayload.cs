using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model;

namespace ThinkingHome.Alice.Handlers.DevicesQuery
{
    public class DevicesQueryPayload
    {
        [JsonPropertyName("devices")] public DeviceState[] Devices { get; set; }
    }
}