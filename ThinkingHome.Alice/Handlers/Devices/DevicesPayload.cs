using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model;

namespace ThinkingHome.Alice.Handlers.Devices
{
    public class DevicesPayload
    {
        [JsonPropertyName("user_id")] public string UserId { get; set; }

        [JsonPropertyName("devices")] public Device[] Devices { get; set; }
    }
}