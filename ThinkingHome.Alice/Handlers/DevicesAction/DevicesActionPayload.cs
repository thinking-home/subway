using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.ActionResult;

namespace ThinkingHome.Alice.Handlers.DevicesAction;

public class DevicesActionPayload
{
    [JsonPropertyName("devices")] public DeviceActionResult[] Devices { get; set; }
}