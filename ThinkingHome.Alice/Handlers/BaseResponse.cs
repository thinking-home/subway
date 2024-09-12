using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers;

public abstract class BaseResponse
{
    [JsonPropertyName("request_id")] public string RequestId { get; set; }
}