using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.ActionResult;
using ThinkingHome.Alice.Model.Capabilities;

namespace ThinkingHome.Alice.Handlers.DevicesAction;

/// <summary>Результат обработки action-запроса для одного устройства.</summary>
public class DeviceActionResult
{
    /// <summary>Идентификатор устройства в терминах провайдера.</summary>
    [JsonPropertyName("id")] public string Id { get; set; } // id устройства
    /// <summary>Общий результат выполнения для устройства целиком.</summary>
    [JsonPropertyName("action_result")] public ActionResult ActionResult { get; set; } // общий код ответа
    /// <summary>Результаты выполнения по отдельным способностям.</summary>
    [JsonPropertyName("capabilities")] public CapabilityActionResultBase[] Capabilities { get; set; }
}