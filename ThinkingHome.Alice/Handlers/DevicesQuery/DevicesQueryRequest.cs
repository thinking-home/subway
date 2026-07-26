using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers.DevicesQuery;

/// <summary>Query-запрос /user/devices/query — запрос текущего состояния устройств.</summary>
public class DevicesQueryRequest
{
    /// <summary>Устройства, состояние которых запрашивается.</summary>
    [JsonPropertyName("devices")] public DeviceReference[] Devices { get; set; }
}