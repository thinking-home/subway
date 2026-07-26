using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers.DevicesAction;

/// <summary>Action-запрос /user/devices/action — изменение состояния устройств.</summary>
public class DevicesActionRequest
{
    /// <summary>Данные запроса: устройства и целевые состояния их способностей.</summary>
    [JsonPropertyName("payload")] public DevicesActionRequestPayload Payload { get; set; }
}