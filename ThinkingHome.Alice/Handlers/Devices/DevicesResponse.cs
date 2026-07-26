using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers.Devices
{
    /// <summary>Ответ на discovery-запрос /user/devices — информация об устройствах пользователя.</summary>
    public class DevicesResponse : BaseResponse
    {
        /// <summary>Данные ответа: пользователь и его устройства.</summary>
        [JsonPropertyName("payload")] public DevicesPayload Payload { get; set; }
    }
}