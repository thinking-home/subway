using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Capabilities;
using ThinkingHome.Alice.Model.Properties;

namespace ThinkingHome.Alice.Model;

/// <summary>Состояние устройства в ответе на query-запрос и в callback-уведомлениях.</summary>
public class DeviceState
{
    /// <summary>Идентификатор устройства в терминах провайдера.</summary>
    [JsonPropertyName("id")] public string Id { get; set; }

    /// <summary>Состояния способностей устройства.</summary>
    [JsonPropertyName("capabilities")] public CapabilityStateBase[] Capabilities { get; set; }
    /// <summary>Состояния свойств устройства.</summary>
    [JsonPropertyName("properties")] public PropertyStateBase[] Properties { get; set; }

    /// <summary>Код ошибки, если состояние устройства получить не удалось.</summary>
    [JsonPropertyName("error_code")] public string ErrorCode { get; set; }
    /// <summary>Текстовое описание ошибки.</summary>
    [JsonPropertyName("error_message")] public string ErrorMessage { get; set; }
}