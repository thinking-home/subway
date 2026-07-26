using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Capabilities;

namespace ThinkingHome.Alice.Handlers.DevicesAction
{
    /// <summary>Устройство в action-запросе: идентификатор и целевые состояния способностей.</summary>
    public class DeviceActionParams
    {
        /// <summary>Идентификатор устройства в терминах провайдера.</summary>
        [JsonPropertyName("id")] public string Id { get; set; }
        /// <summary>Произвольные данные провайдера, переданные при discovery; Алиса возвращает их как есть.</summary>
        [JsonPropertyName("custom_data")] public object CustomData { get; set; }
        /// <summary>Способности, состояние которых нужно изменить.</summary>
        [JsonPropertyName("capabilities")] public CapabilityActionParamsBase[] Capabilities { get; set; }
    }
}