using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.ActionResult;

namespace ThinkingHome.Alice.Handlers.DevicesAction;

/// <summary>Данные ответа на action-запрос: результаты выполнения по каждому устройству.</summary>
public class DevicesActionPayload
{
    /// <summary>Результаты выполнения команд по устройствам.</summary>
    [JsonPropertyName("devices")] public DeviceActionResult[] Devices { get; set; }
}