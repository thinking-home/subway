using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers.DevicesAction;

public class DevicesActionRequestPayload
{
    [JsonPropertyName("devices")] public DeviceActionParams[] Devices { get; set; }
}