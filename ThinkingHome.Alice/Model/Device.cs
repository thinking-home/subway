using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Capabilities;
using ThinkingHome.Alice.Model.Properties;

namespace ThinkingHome.Alice.Model;

/// <summary>Описание устройства в ответе на discovery: тип, способности, свойства.</summary>
public class Device
{
    /// <summary>Идентификатор устройства в терминах провайдера.</summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>Название устройства.</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>Описание устройства.</summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>Комната, в которой находится устройство.</summary>
    [JsonPropertyName("room")]
    public string Room { get; set; }

    /// <summary>Тип устройства (devices.types.*).</summary>
    [JsonPropertyName("type")]
    public DeviceType Type { get; set; }

    /// <summary>Произвольные данные провайдера; Алиса возвращает их как есть в query/action-запросах.</summary>
    [JsonPropertyName("custom_data")]
    public object CustomData { get; set; }

    /// <summary>Способности устройства — то, чем можно управлять.</summary>
    [JsonPropertyName("capabilities")]
    public CapabilityInfoBase[] Capabilities { get; set; }

    /// <summary>Свойства устройства — показания сенсоров и события (только чтение).</summary>
    [JsonPropertyName("properties")]
    public PropertyInfoBase[] Properties { get; set; }

    /// <summary>Сведения о производителе, модели и версиях устройства.</summary>
    [JsonPropertyName("device_info")]
    public DeviceInfo DeviceInfo { get; set; }
}