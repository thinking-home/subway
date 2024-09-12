using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers.DevicesQuery
{
    public class DevicesQueryResponse : BaseResponse
    {
        [JsonPropertyName("payload")] public DevicesQueryPayload Payload { get; set; }
    }
}