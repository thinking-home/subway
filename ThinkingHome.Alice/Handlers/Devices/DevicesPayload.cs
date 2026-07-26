using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model;

namespace ThinkingHome.Alice.Handlers.Devices
{
    /// <summary>Данные ответа на discovery-запрос: пользователь и список его устройств.</summary>
    public class DevicesPayload
    {
        /// <summary>Идентификатор пользователя в системе провайдера.</summary>
        [JsonPropertyName("user_id")] public string UserId { get; set; }

        /// <summary>Устройства пользователя.</summary>
        [JsonPropertyName("devices")] public Device[] Devices { get; set; }
    }
}