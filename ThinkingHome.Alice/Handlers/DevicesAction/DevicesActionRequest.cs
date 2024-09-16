using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers.DevicesAction;

public class DevicesActionRequest
{
    [JsonPropertyName("payload")] public DevicesActionRequestPayload Payload { get; set; }
}