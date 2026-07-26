using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers.DevicesAction;

/// <summary>Ответ на action-запрос /user/devices/action.</summary>
public class DevicesActionResponse: BaseResponse
{
    /// <summary>Данные ответа: результаты выполнения по каждому устройству.</summary>
    [JsonPropertyName("payload")] public DevicesActionPayload Payload { get; set; }
}