using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers.DevicesAction;

/// <summary>Данные action-запроса: список устройств с целевыми состояниями способностей.</summary>
public class DevicesActionRequestPayload
{
    /// <summary>Устройства, состояние которых нужно изменить.</summary>
    [JsonPropertyName("devices")] public DeviceActionParams[] Devices { get; set; }
}