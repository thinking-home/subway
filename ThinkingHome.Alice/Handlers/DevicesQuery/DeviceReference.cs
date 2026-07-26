using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers.DevicesQuery
{
    /// <summary>Ссылка на устройство в query-запросе.</summary>
    public class DeviceReference
    {
        /// <summary>Идентификатор устройства в терминах провайдера.</summary>
        [JsonPropertyName("id")] public string Id { get; set; }
        /// <summary>Произвольные данные провайдера, переданные при discovery; Алиса возвращает их как есть.</summary>
        [JsonPropertyName("custom_data")] public object CustomData { get; set; }
    }
}