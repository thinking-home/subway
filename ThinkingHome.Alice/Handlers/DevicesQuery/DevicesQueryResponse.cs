using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers.DevicesQuery
{
    /// <summary>Ответ на query-запрос /user/devices/query.</summary>
    public class DevicesQueryResponse : BaseResponse
    {
        /// <summary>Данные ответа: состояния устройств.</summary>
        [JsonPropertyName("payload")] public DevicesQueryPayload Payload { get; set; }
    }
}