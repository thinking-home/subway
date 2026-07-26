using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model;

namespace ThinkingHome.Alice.Handlers.DevicesQuery
{
    /// <summary>Данные ответа на query-запрос: состояния запрошенных устройств.</summary>
    public class DevicesQueryPayload
    {
        /// <summary>Состояния устройств.</summary>
        [JsonPropertyName("devices")] public DeviceState[] Devices { get; set; }
    }
}