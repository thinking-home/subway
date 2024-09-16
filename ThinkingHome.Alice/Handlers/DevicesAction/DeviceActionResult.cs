using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.ActionResult;
using ThinkingHome.Alice.Model.Capabilities;

namespace ThinkingHome.Alice.Handlers.DevicesAction;

public class DeviceActionResult
{
    [JsonPropertyName("id")] public string Id { get; set; } // id устройства
    [JsonPropertyName("action_result")] public ActionResult ActionResult { get; set; } // общий код ответа
    [JsonPropertyName("capabilities")] public CapabilityActionResultBase[] Capabilities { get; set; }
}