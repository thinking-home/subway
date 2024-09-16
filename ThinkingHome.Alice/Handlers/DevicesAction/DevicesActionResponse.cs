using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers.DevicesAction;

public class DevicesActionResponse: BaseResponse
{
    [JsonPropertyName("payload")] public DevicesActionPayload Payload { get; set; }
}