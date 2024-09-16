using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers.Devices
{
    public class DevicesResponse : BaseResponse
    {
        [JsonPropertyName("payload")] public DevicesPayload Payload { get; set; }
    }
}